using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Roommate;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class RoommateMatchingService : IRoommateMatchingService
    {
        private readonly MARN_API.Repositories.Interfaces.IRoommatePreferenceRepo _repo;

        public RoommateMatchingService(MARN_API.Repositories.Interfaces.IRoommatePreferenceRepo repo)
        {
            _repo = repo;
        }

        private double[] GetProfileVector(RoommatePreference pref)
        {
            double[] vec = new double[24];
            
            // 0: Smoking
            vec[0] = pref.Smoking.HasValue ? (pref.Smoking.Value ? 1.0 : -1.0) : 0.0;
            
            // 1: Pets
            vec[1] = pref.Pets.HasValue ? (pref.Pets.Value ? 1.0 : -1.0) : 0.0;
            
            // 2: SleepSchedule
            if (pref.SleepSchedule == SleepSchedule.EarlyBird) vec[2] = -1.0;
            else if (pref.SleepSchedule == SleepSchedule.NightOwl) vec[2] = 1.0;
            else vec[2] = 0.0; 

            // 3: EducationLevel (HighSchool=1, Bachelor=2, Master=3, Doctorate=4)
            if (pref.EducationLevel == EducationLevel.HighSchool) vec[3] = -1.0;
            else if (pref.EducationLevel == EducationLevel.Bachelor) vec[3] = -0.33;
            else if (pref.EducationLevel == EducationLevel.Master) vec[3] = 0.33;
            else if (pref.EducationLevel == EducationLevel.Doctorate) vec[3] = 1.0;
            else vec[3] = 0.0;

            // 4: NoiseTolerance (1 to 5)
            if (pref.NoiseTolerance.HasValue) vec[4] = (pref.NoiseTolerance.Value - 3) / 2.0;

            // 5: GuestsFrequency (Never=1, Rarely=2, Sometimes=3, Often=4)
            if (pref.GuestsFrequency == GuestsFrequency.Never) vec[5] = -1.0;
            else if (pref.GuestsFrequency == GuestsFrequency.Rarely) vec[5] = -0.33;
            else if (pref.GuestsFrequency == GuestsFrequency.Sometimes) vec[5] = 0.33;
            else if (pref.GuestsFrequency == GuestsFrequency.Often) vec[5] = 1.0;

            // 6: SharingLevel (Low=1, Medium=2, High=3)
            if (pref.SharingLevel == SharingLevel.Low) vec[6] = -1.0;
            else if (pref.SharingLevel == SharingLevel.High) vec[6] = 1.0;
            else vec[6] = 0.0; 

            // Field of Study (7 to 17)
            if (pref.FieldOfStudy != FieldOfStudy.Unknown)
            {
                int fieldIndex = 7 + (pref.FieldOfStudy == FieldOfStudy.Other ? 10 : (int)pref.FieldOfStudy - 1);
                if (fieldIndex >= 7 && fieldIndex <= 17) vec[fieldIndex] = 1.0;
            }

            // Work Schedule (18 to 23)
            if (pref.WorkSchedule != WorkSchedule.Unknown)
            {
                int workIndex = 18 + (int)pref.WorkSchedule - 1;
                if (workIndex >= 18 && workIndex <= 23) vec[workIndex] = 1.0;
            }

            return vec;
        }

        private double[] GetWeightVector(RoommatePreference pref)
        {
            double[] w = new double[24];
            w[0] = (int)pref.SmokingImportance;
            w[1] = (int)pref.PetsImportance;
            w[2] = (int)pref.SleepImportance;
            w[3] = (int)pref.EducationImportance;
            w[4] = (int)pref.NoiseToleranceImportance;
            w[5] = (int)pref.GuestsFrequencyImportance;
            w[6] = (int)pref.SharingLevelImportance;
            
            double fieldW = (int)pref.FieldOfStudyImportance;
            for (int i = 7; i <= 17; i++) w[i] = fieldW;

            double workW = (int)pref.WorkScheduleImportance;
            for (int i = 18; i <= 23; i++) w[i] = workW;

            return w;
        }

        private (double, double) CalculateBudgetOverlap(RoommatePreference a, RoommatePreference b)
        {
            if (!a.BudgetRangeMin.HasValue || !a.BudgetRangeMax.HasValue || !b.BudgetRangeMin.HasValue || !b.BudgetRangeMax.HasValue)
                return (0, 0);

            var overlapStart = Math.Max(a.BudgetRangeMin.Value, b.BudgetRangeMin.Value);
            var overlapEnd = Math.Min(a.BudgetRangeMax.Value, b.BudgetRangeMax.Value);
            
            if (overlapStart <= overlapEnd)
            {
                double rangeA = (double)Math.Max(1m, a.BudgetRangeMax.Value - a.BudgetRangeMin.Value);
                double rangeB = (double)Math.Max(1m, b.BudgetRangeMax.Value - b.BudgetRangeMin.Value);
                double overlap = (double)(overlapEnd - overlapStart);
                return (Math.Min(1.0, overlap / rangeA), Math.Min(1.0, overlap / rangeB));
            }
            return (0, 0);
        }

        private double CalculateWeightedCosineSimilarity(double[] vecA, double[] vecB, double[] weights, double budgetRatio, double budgetWeight)
        {
            double dotProduct = 0;
            double normA = 0;
            double normB = 0;

            for (int i = 0; i < vecA.Length; i++)
            {
                dotProduct += weights[i] * vecA[i] * vecB[i];
                normA += weights[i] * vecA[i] * vecA[i];
                normB += weights[i] * vecB[i] * vecB[i];
            }

            dotProduct += budgetWeight * budgetRatio;
            normA += budgetWeight * 1.0; // Max possible budget ratio is 1.0
            normB += budgetWeight * 1.0;

            if (normA == 0 || normB == 0) return 0;

            double similarity = dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
            return Math.Max(0, similarity); // Clamp negative similarity to 0
        }

        public async Task<IEnumerable<RoommateMatchDto>> GetTopMatchesAsync(Guid currentUserId, int k = 10)
        {
            var currentUserPref = await _repo.GetRoommatePreferences(currentUserId);

            if (currentUserPref == null || !currentUserPref.RoommatePreferencesEnabled)
            {
                return new List<RoommateMatchDto>();
            }

            var potentialMatches = await _repo.GetPotentialMatchesAsync(currentUserId, currentUserPref.Governorate, currentUserPref.User.Gender);
            var matchedResults = new List<RoommateMatchDto>();

            double[] baseVecA = GetProfileVector(currentUserPref);
            double[] weightsA = GetWeightVector(currentUserPref);
            double budgetWeightA = (int)currentUserPref.BudgetImportance;

            foreach (var matchPref in potentialMatches)
            {
                double[] vecA = (double[])baseVecA.Clone();
                double[] vecB = GetProfileVector(matchPref);
                double[] weightsB = GetWeightVector(matchPref);
                double budgetWeightB = (int)matchPref.BudgetImportance;

                // Handle Flexible Wildcard
                if (currentUserPref.SleepSchedule == SleepSchedule.Flexible && matchPref.SleepSchedule != SleepSchedule.Unknown)
                    vecA[2] = vecB[2];
                else if (matchPref.SleepSchedule == SleepSchedule.Flexible && currentUserPref.SleepSchedule != SleepSchedule.Unknown)
                    vecB[2] = vecA[2];

                var (budgetRatioA, budgetRatioB) = CalculateBudgetOverlap(currentUserPref, matchPref);
                
                double similarityA = CalculateWeightedCosineSimilarity(vecA, vecB, weightsA, budgetRatioA, budgetWeightA);
                double similarityB = CalculateWeightedCosineSimilarity(vecB, vecA, weightsB, budgetRatioB, budgetWeightB);
                
                double mutualSimilarity = Math.Sqrt(similarityA * similarityB);
                double rawScore = mutualSimilarity * 100.0;
                double penalty = 0;

                var matchedTraits = new List<string>();
                var mismatchedTraits = new List<string>();
                var dealbreakers = new List<string>();

                // Smoking (Binary)
                if (currentUserPref.Smoking.HasValue && matchPref.Smoking.HasValue)
                {
                    double score = currentUserPref.Smoking == matchPref.Smoking ? 1.0 : 0.0;
                    if (score == 1.0) matchedTraits.Add(currentUserPref.Smoking.Value ? "Both Smoke" : "Both Non-Smokers");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Smoking Preference");
                        if (currentUserPref.SmokingImportance == 5)
                        {
                            dealbreakers.Add("Smoking mismatch");
                            penalty += 40; 
                        }
                        if (matchPref.SmokingImportance == 5) penalty += 40;
                    }
                }

                // Pets (Binary)
                if (currentUserPref.Pets.HasValue && matchPref.Pets.HasValue)
                {
                    double score = currentUserPref.Pets == matchPref.Pets ? 1.0 : 0.0;
                    if (score == 1.0) matchedTraits.Add(currentUserPref.Pets.Value ? "Both love pets" : "Both prefer no pets");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Pets Preference");
                        if (currentUserPref.PetsImportance == 5)
                        {
                            dealbreakers.Add("Pets mismatch");
                            penalty += 40;
                        }
                        if (matchPref.PetsImportance == 5) penalty += 40;
                    }
                }

                // Sleep Schedule (Special Ordinal)
                if (currentUserPref.SleepSchedule != SleepSchedule.Unknown && matchPref.SleepSchedule != SleepSchedule.Unknown)
                {
                    double score = 0;
                    if (currentUserPref.SleepSchedule == matchPref.SleepSchedule || 
                        currentUserPref.SleepSchedule == SleepSchedule.Flexible || 
                        matchPref.SleepSchedule == SleepSchedule.Flexible)
                    {
                        score = 1.0;
                    }
                    else
                    {
                        int diff = Math.Abs((int)currentUserPref.SleepSchedule - (int)matchPref.SleepSchedule);
                        score = 1.0 - diff; 
                    }

                    if (score == 1.0) matchedTraits.Add("Compatible Sleep Schedule");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Sleep Schedule");
                        if (currentUserPref.SleepImportance == 5)
                        {
                            dealbreakers.Add("Sleep Schedule mismatch");
                            penalty += 40;
                        }
                        if (matchPref.SleepImportance == 5) penalty += 40;
                    }
                }

                // Education Level (Linear Ordinal - Max Diff 3)
                if (currentUserPref.EducationLevel != EducationLevel.Unknown && matchPref.EducationLevel != EducationLevel.Unknown)
                {
                    int diff = Math.Abs((int)currentUserPref.EducationLevel - (int)matchPref.EducationLevel);
                    double score = 1.0 - (diff / 3.0); 

                    if (score >= 0.8) matchedTraits.Add("Similar Education Level");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Education Level");
                        if (currentUserPref.EducationImportance == 5)
                        {
                            dealbreakers.Add("Education Level mismatch");
                            penalty += 40;
                        }
                        if (matchPref.EducationImportance == 5) penalty += 40;
                    }
                }

                // Field of Study (Strict Categorical)
                if (currentUserPref.FieldOfStudy != FieldOfStudy.Unknown && matchPref.FieldOfStudy != FieldOfStudy.Unknown)
                {
                    double score = currentUserPref.FieldOfStudy == matchPref.FieldOfStudy ? 1.0 : 0.0;

                    if (score == 1.0) matchedTraits.Add("Same Field of Study");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Field of Study");
                        if (currentUserPref.FieldOfStudyImportance == 5)
                        {
                            dealbreakers.Add("Field of Study mismatch");
                            penalty += 40;
                        }
                        if (matchPref.FieldOfStudyImportance == 5) penalty += 40;
                    }
                }

                // Noise Tolerance (Linear Ordinal - Max Diff 4)
                if (currentUserPref.NoiseTolerance.HasValue && matchPref.NoiseTolerance.HasValue)
                {
                    int diff = Math.Abs(currentUserPref.NoiseTolerance.Value - matchPref.NoiseTolerance.Value);
                    double score = 1.0 - (diff / 4.0);
                    
                    if (score >= 0.75) matchedTraits.Add("Similar Noise Tolerance");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Noise Tolerance");
                        if (currentUserPref.NoiseToleranceImportance == 5)
                        {
                            dealbreakers.Add("Noise Tolerance mismatch");
                            penalty += 40;
                        }
                        if (matchPref.NoiseToleranceImportance == 5) penalty += 40;
                    }
                }
                
                // Guests Frequency (Linear Ordinal - Max Diff 3)
                if (currentUserPref.GuestsFrequency != GuestsFrequency.Unknown && matchPref.GuestsFrequency != GuestsFrequency.Unknown)
                {
                    int diff = Math.Abs((int)currentUserPref.GuestsFrequency - (int)matchPref.GuestsFrequency);
                    double score = 1.0 - (diff / 3.0);
                    
                    if (score >= 0.7) matchedTraits.Add("Similar Guests Preference");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Guests Frequency");
                        if (currentUserPref.GuestsFrequencyImportance == 5)
                        {
                            dealbreakers.Add("Guests Frequency mismatch");
                            penalty += 40;
                        }
                        if (matchPref.GuestsFrequencyImportance == 5) penalty += 40;
                    }
                }

                // Sharing Level (Linear Ordinal - Max Diff 2)
                if (currentUserPref.SharingLevel != SharingLevel.Unknown && matchPref.SharingLevel != SharingLevel.Unknown)
                {
                    int diff = Math.Abs((int)currentUserPref.SharingLevel - (int)matchPref.SharingLevel);
                    double score = 1.0 - (diff / 2.0);

                    if (score < 0.5)
                    {
                        mismatchedTraits.Add("Sharing Level");
                        if (currentUserPref.SharingLevelImportance == 5)
                        {
                            dealbreakers.Add("Sharing Level mismatch");
                            penalty += 40;
                        }
                        if (matchPref.SharingLevelImportance == 5) penalty += 40;
                    }
                }

                // Work Schedule (Strict Categorical)
                if (currentUserPref.WorkSchedule != WorkSchedule.Unknown && matchPref.WorkSchedule != WorkSchedule.Unknown)
                {
                    double score = currentUserPref.WorkSchedule == matchPref.WorkSchedule ? 1.0 : 0.0;

                    if (score == 1.0) matchedTraits.Add("Same Work Schedule");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Work Schedule");
                        if (currentUserPref.WorkScheduleImportance == 5)
                        {
                            dealbreakers.Add("Work Schedule mismatch");
                            penalty += 40;
                        }
                        if (matchPref.WorkScheduleImportance == 5) penalty += 40;
                    }
                }

                // Budget Overlap
                if (currentUserPref.BudgetRangeMin.HasValue && currentUserPref.BudgetRangeMax.HasValue &&
                    matchPref.BudgetRangeMin.HasValue && matchPref.BudgetRangeMax.HasValue)
                {
                    if (budgetRatioA >= 0.5 && budgetRatioB >= 0.5) matchedTraits.Add("Compatible Budget");
                    else if (budgetRatioA < 0.5)
                    {
                        mismatchedTraits.Add("Budget");
                        if (currentUserPref.BudgetImportance == 5)
                        {
                            dealbreakers.Add("Insufficient Budget overlap");
                            penalty += 40;
                        }
                    }
                    if (budgetRatioB < 0.5 && matchPref.BudgetImportance == 5) penalty += 40;
                }
                else 
                {
                    if (currentUserPref.BudgetImportance == 5)
                    {
                        dealbreakers.Add("Budget Mismatch");
                        penalty += 40;
                    }
                    if (matchPref.BudgetImportance == 5) penalty += 40;
                }

                // Calculate final score
                double finalScore = Math.Max(0, rawScore - penalty);

                // Badge Logic
                string badge = string.Empty;
                if (currentUserPref.SearchStatus == RoommateSearchStatus.Searching && matchPref.SearchStatus == RoommateSearchStatus.Searching)
                    badge = "Let's Find a Place";
                else if (currentUserPref.SearchStatus == RoommateSearchStatus.Searching && matchPref.SearchStatus == RoommateSearchStatus.Offering)
                    badge = "Has Apartment";
                else if (currentUserPref.SearchStatus == RoommateSearchStatus.Offering && matchPref.SearchStatus == RoommateSearchStatus.Searching)
                    badge = "Looking for a Room";

                matchedResults.Add(new RoommateMatchDto
                {
                    UserId = matchPref.UserId,
                    FullName = $"{matchPref.User.FirstName} {matchPref.User.LastName}".Trim(),
                    ProfileImage = matchPref.User.ProfileImage,
                    SearchStatus = matchPref.SearchStatus,
                    Badge = badge,
                    CompatibilityScore = Math.Round(finalScore, 1),
                    TopMatchingTraits = matchedTraits,
                    MismatchedTraits = mismatchedTraits,
                    DealbreakersFound = dealbreakers
                });
            }

            return matchedResults.OrderByDescending(x => x.CompatibilityScore).Take(k);
        }
    }
}

