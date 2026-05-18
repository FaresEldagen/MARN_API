using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MARN_API.DTOs.Roommate;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class RoommateMatchingService : IRoommateMatchingService
    {
        private const int MinImportance = 1;
        private const int MaxImportance = 5;
        private const int DealbreakerImportance = 5;

        private readonly MARN_API.Repositories.Interfaces.IRoommatePreferenceRepo _repo;
        private readonly IAppTextLocalizer _localizer;

        public RoommateMatchingService(
            MARN_API.Repositories.Interfaces.IRoommatePreferenceRepo repo,
            IAppTextLocalizer localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        private readonly record struct ScoredFeature(double Score, double Weight);

        private static int ClampImportance(int importance)
        {
            return Math.Clamp(importance, MinImportance, MaxImportance);
        }

        private static double CalculateMutualWeight(int importanceA, int importanceB)
        {
            return Math.Sqrt(ClampImportance(importanceA) * ClampImportance(importanceB));
        }

        private static double CalculateOrdinalScore(int valueA, int valueB, int minValue, int maxValue)
        {
            int maxDifference = maxValue - minValue;
            if (maxDifference <= 0) return 1.0;

            double difference = Math.Abs(valueA - valueB);
            return Math.Clamp(1.0 - (difference / maxDifference), 0.0, 1.0);
        }

        private static double CalculateBudgetOverlapScore(RoommatePreference a, RoommatePreference b)
        {
            if (!a.BudgetRangeMin.HasValue || !a.BudgetRangeMax.HasValue || !b.BudgetRangeMin.HasValue || !b.BudgetRangeMax.HasValue)
                return 0.0;

            decimal overlapStart = Math.Max(a.BudgetRangeMin.Value, b.BudgetRangeMin.Value);
            decimal overlapEnd = Math.Min(a.BudgetRangeMax.Value, b.BudgetRangeMax.Value);

            if (overlapStart > overlapEnd)
                return 0.0;

            decimal rangeA = a.BudgetRangeMax.Value - a.BudgetRangeMin.Value;
            decimal rangeB = b.BudgetRangeMax.Value - b.BudgetRangeMin.Value;

            if (rangeA == 0 && rangeB == 0)
                return a.BudgetRangeMin.Value == b.BudgetRangeMin.Value ? 1.0 : 0.0;

            if (rangeA == 0)
                return b.BudgetRangeMin.Value <= a.BudgetRangeMin.Value && a.BudgetRangeMin.Value <= b.BudgetRangeMax.Value ? 1.0 : 0.0;

            if (rangeB == 0)
                return a.BudgetRangeMin.Value <= b.BudgetRangeMin.Value && b.BudgetRangeMin.Value <= a.BudgetRangeMax.Value ? 1.0 : 0.0;

            decimal overlap = overlapEnd - overlapStart;
            decimal smallerRange = Math.Min(rangeA, rangeB);
            return Math.Clamp((double)(overlap / smallerRange), 0.0, 1.0);
        }

        private static void AddFeatureScore(
            List<ScoredFeature> features,
            List<string> matchedTraits,
            List<string> mismatchedTraits,
            List<string> dealbreakers,
            double score,
            int importanceA,
            int importanceB,
            string? matchedTrait,
            string mismatchTrait,
            string dealbreakerTrait,
            double matchedThreshold = 1.0,
            double mismatchThreshold = 0.5)
        {
            score = Math.Clamp(score, 0.0, 1.0);
            features.Add(new ScoredFeature(score, CalculateMutualWeight(importanceA, importanceB)));

            if (!string.IsNullOrWhiteSpace(matchedTrait) && score >= matchedThreshold)
            {
                matchedTraits.Add(matchedTrait);
            }
            else if (score < mismatchThreshold)
            {
                mismatchedTraits.Add(mismatchTrait);

                if (ClampImportance(importanceA) == DealbreakerImportance || ClampImportance(importanceB) == DealbreakerImportance)
                {
                    dealbreakers.Add(dealbreakerTrait);
                }
            }
        }

        private static double CalculateFinalScore(IEnumerable<ScoredFeature> features)
        {
            double totalWeight = features.Sum(f => f.Weight);
            if (totalWeight <= 0) return 0.0;

            double weightedScore = features.Sum(f => f.Score * f.Weight);
            return Math.Clamp((weightedScore / totalWeight) * 100.0, 0.0, 100.0);
        }

        public async Task<ServiceResult<IEnumerable<RoommateMatchDto>>> GetTopMatchesAsync(Guid currentUserId, int k = 10)
        {
            var currentUserPref = await _repo.GetRoommatePreferences(currentUserId);

            if (currentUserPref == null || !currentUserPref.RoommatePreferencesEnabled)
            {
                return ServiceResult<IEnumerable<RoommateMatchDto>>.Ok(new List<RoommateMatchDto>(), "Roommate matching preferences not found or disabled.");
            }

            var potentialMatches = await _repo.GetPotentialMatchesAsync(currentUserId, currentUserPref.Governorate, currentUserPref.User.Gender);
            var matchedResults = new List<RoommateMatchDto>();

            foreach (var matchPref in potentialMatches)
            {
                var result = CalculateMatchResult(currentUserPref, matchPref);
                if (result != null)
                {
                    matchedResults.Add(result);
                }
            }

            return ServiceResult<IEnumerable<RoommateMatchDto>>.Ok(matchedResults.OrderByDescending(x => x.CompatibilityScore).Take(k));
        }

        public async Task<ServiceResult<Dictionary<Guid, RoommateMatchDto>>> GetMatchScoresAsync(Guid currentUserId, List<Guid> targetUserIds)
        {
            if (targetUserIds == null || !targetUserIds.Any())
                return ServiceResult<Dictionary<Guid, RoommateMatchDto>>.Ok(new Dictionary<Guid, RoommateMatchDto>());

            var currentUserPref = await _repo.GetRoommatePreferences(currentUserId);
            var targetPrefs = await _repo.GetPreferencesInBatchAsync(targetUserIds);

            var results = new Dictionary<Guid, RoommateMatchDto>();

            foreach (var targetId in targetUserIds)
            {
                var matchPref = targetPrefs.FirstOrDefault(p => p.UserId == targetId);
                
                if (currentUserPref == null || !currentUserPref.RoommatePreferencesEnabled || 
                    matchPref == null || !matchPref.RoommatePreferencesEnabled)
                {
                    results[targetId] = new RoommateMatchDto { UserId = targetId, CompatibilityScore = 0 };
                    continue;
                }

                var matchResult = CalculateMatchResult(currentUserPref, matchPref);
                if (matchResult != null)
                {
                    results[targetId] = matchResult;
                }
            }

            return ServiceResult<Dictionary<Guid, RoommateMatchDto>>.Ok(results);
        }

        private RoommateMatchDto CalculateMatchResult(RoommatePreference currentUserPref, RoommatePreference matchPref)
        {
            var features = new List<ScoredFeature>();
            var matchedTraits = new List<string>();
            var mismatchedTraits = new List<string>();
            var dealbreakers = new List<string>();

            // Smoking (Binary)
            if (currentUserPref.Smoking.HasValue && matchPref.Smoking.HasValue)
            {
                double score = currentUserPref.Smoking == matchPref.Smoking ? 1.0 : 0.0;
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.SmokingImportance,
                    matchPref.SmokingImportance,
                    currentUserPref.Smoking.Value ? T("Both Smoke") : T("Both Non-Smokers"),
                    T("Smoking Preference"),
                    T("Smoking mismatch"));
            }

            // Pets (Binary)
            if (currentUserPref.Pets.HasValue && matchPref.Pets.HasValue)
            {
                double score = currentUserPref.Pets == matchPref.Pets ? 1.0 : 0.0;
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.PetsImportance,
                    matchPref.PetsImportance,
                    currentUserPref.Pets.Value ? T("Both love pets") : T("Both prefer no pets"),
                    T("Pets Preference"),
                    T("Pets mismatch"));
            }

            // Sleep Schedule (Flexible is a wildcard, not an ordinal midpoint)
            if (currentUserPref.SleepSchedule != SleepSchedule.Unknown && matchPref.SleepSchedule != SleepSchedule.Unknown)
            {
                double score;
                if (currentUserPref.SleepSchedule == matchPref.SleepSchedule || 
                    currentUserPref.SleepSchedule == SleepSchedule.Flexible || 
                    matchPref.SleepSchedule == SleepSchedule.Flexible)
                {
                    score = 1.0;
                }
                else
                {
                    score = 0.0;
                }

                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.SleepImportance,
                    matchPref.SleepImportance,
                    T("Compatible Sleep Schedule"),
                    T("Sleep Schedule"),
                    T("Sleep Schedule mismatch"));
            }

            // Education Level (Linear Ordinal - Max Diff 3)
            if (currentUserPref.EducationLevel != EducationLevel.Unknown && matchPref.EducationLevel != EducationLevel.Unknown)
            {
                double score = CalculateOrdinalScore((int)currentUserPref.EducationLevel, (int)matchPref.EducationLevel, (int)EducationLevel.HighSchool, (int)EducationLevel.Doctorate);
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.EducationImportance,
                    matchPref.EducationImportance,
                    T("Similar Education Level"),
                    T("Education Level"),
                    T("Education Level mismatch"),
                    matchedThreshold: 0.8);
            }

            // Field of Study (Strict Categorical)
            if (currentUserPref.FieldOfStudy != FieldOfStudy.Unknown && matchPref.FieldOfStudy != FieldOfStudy.Unknown)
            {
                double score = currentUserPref.FieldOfStudy == matchPref.FieldOfStudy ? 1.0 : 0.0;
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.FieldOfStudyImportance,
                    matchPref.FieldOfStudyImportance,
                    T("Same Field of Study"),
                    T("Field of Study"),
                    T("Field of Study mismatch"));
            }

            // Noise Tolerance (Linear Ordinal - Max Diff 4)
            if (currentUserPref.NoiseTolerance.HasValue && matchPref.NoiseTolerance.HasValue)
            {
                double score = CalculateOrdinalScore(currentUserPref.NoiseTolerance.Value, matchPref.NoiseTolerance.Value, 1, 5);
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.NoiseToleranceImportance,
                    matchPref.NoiseToleranceImportance,
                    T("Similar Noise Tolerance"),
                    T("Noise Tolerance"),
                    T("Noise Tolerance mismatch"),
                    matchedThreshold: 0.75);
            }
            
            // Guests Frequency (Linear Ordinal - Max Diff 3)
            if (currentUserPref.GuestsFrequency != GuestsFrequency.Unknown && matchPref.GuestsFrequency != GuestsFrequency.Unknown)
            {
                double score = CalculateOrdinalScore((int)currentUserPref.GuestsFrequency, (int)matchPref.GuestsFrequency, (int)GuestsFrequency.Never, (int)GuestsFrequency.Often);
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.GuestsFrequencyImportance,
                    matchPref.GuestsFrequencyImportance,
                    T("Similar Guests Preference"),
                    T("Guests Frequency"),
                    T("Guests Frequency mismatch"),
                    matchedThreshold: 0.7);
            }

            // Sharing Level (Linear Ordinal - Max Diff 2)
            if (currentUserPref.SharingLevel != SharingLevel.Unknown && matchPref.SharingLevel != SharingLevel.Unknown)
            {
                double score = CalculateOrdinalScore((int)currentUserPref.SharingLevel, (int)matchPref.SharingLevel, (int)SharingLevel.Low, (int)SharingLevel.High);
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.SharingLevelImportance,
                    matchPref.SharingLevelImportance,
                    T("Similar Sharing Level"),
                    T("Sharing Level"),
                    T("Sharing Level mismatch"),
                    matchedThreshold: 0.75);
            }

            // Work Schedule (Strict Categorical)
            if (currentUserPref.WorkSchedule != WorkSchedule.Unknown && matchPref.WorkSchedule != WorkSchedule.Unknown)
            {
                double score = currentUserPref.WorkSchedule == matchPref.WorkSchedule ? 1.0 : 0.0;
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.WorkScheduleImportance,
                    matchPref.WorkScheduleImportance,
                    T("Same Work Schedule"),
                    T("Work Schedule"),
                    T("Work Schedule mismatch"));
            }

            // Budget Overlap
            if (currentUserPref.BudgetRangeMin.HasValue && currentUserPref.BudgetRangeMax.HasValue &&
                matchPref.BudgetRangeMin.HasValue && matchPref.BudgetRangeMax.HasValue)
            {
                double score = CalculateBudgetOverlapScore(currentUserPref, matchPref);
                AddFeatureScore(
                    features,
                    matchedTraits,
                    mismatchedTraits,
                    dealbreakers,
                    score,
                    currentUserPref.BudgetImportance,
                    matchPref.BudgetImportance,
                    T("Compatible Budget"),
                    T("Budget"),
                    T("Insufficient Budget overlap"),
                    matchedThreshold: 0.5);
            }

            double finalScore = CalculateFinalScore(features);

            // Badge Logic
            string badge = string.Empty;
            if (currentUserPref.SearchStatus == RoommateSearchStatus.Searching && matchPref.SearchStatus == RoommateSearchStatus.Searching)
                badge = T("Let's Find a Place");
            else if (currentUserPref.SearchStatus == RoommateSearchStatus.Searching && matchPref.SearchStatus == RoommateSearchStatus.Offering)
                badge = T("Has Apartment");
            else if (currentUserPref.SearchStatus == RoommateSearchStatus.Offering && matchPref.SearchStatus == RoommateSearchStatus.Searching)
                badge = T("Looking for a Room");

            return new RoommateMatchDto
            {
                UserId = matchPref.UserId,
                FullName = $"{matchPref.User.FirstName} {matchPref.User.LastName}".Trim(),
                ProfileImage = matchPref.User.ProfileImage,
                SearchStatus = matchPref.SearchStatus,
                SearchStatusDisplayName = _localizer.GetEnumDisplayName(matchPref.SearchStatus),
                Badge = badge,
                CompatibilityScore = Math.Round(finalScore, 1),
                TopMatchingTraits = matchedTraits,
                MismatchedTraits = mismatchedTraits,
                DealbreakersFound = dealbreakers
            };
        }

        private string T(string literal) => _localizer.LocalizeLiteral(literal);
    }
}
