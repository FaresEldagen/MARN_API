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

        public async Task<IEnumerable<RoommateMatchDto>> GetTopMatchesAsync(Guid currentUserId, int k = 10)
        {
            var currentUserPref = await _repo.GetRoommatePreferences(currentUserId);

            if (currentUserPref == null || !currentUserPref.RoommatePreferencesEnabled)
            {
                return new List<RoommateMatchDto>();
            }

            var potentialMatches = await _repo.GetPotentialMatchesAsync(currentUserId, currentUserPref.Governorate, currentUserPref.User.Gender);
            var matchedResults = new List<RoommateMatchDto>();

            foreach (var matchPref in potentialMatches)
            {
                double earnedPoints = 0;
                double totalPossiblePoints = 0;
                var matchedTraits = new List<string>();
                var mismatchedTraits = new List<string>();
                var dealbreakers = new List<string>();
                double penalty = 0;

                // Smoking (Binary)
                double smokingWeight = (int)currentUserPref.SmokingImportance;
                totalPossiblePoints += smokingWeight;
                if (currentUserPref.Smoking.HasValue && matchPref.Smoking.HasValue)
                {
                    double score = currentUserPref.Smoking == matchPref.Smoking ? 1.0 : 0.0;
                    earnedPoints += score * smokingWeight;

                    if (score == 1.0) matchedTraits.Add(currentUserPref.Smoking.Value ? "Both Smoke" : "Both Non-Smokers");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Smoking Preference");
                        if (currentUserPref.SmokingImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Smoking mismatch");
                            penalty += 40; 
                        }
                    }
                }

                // Pets (Binary)
                double petsWeight = (int)currentUserPref.PetsImportance;
                totalPossiblePoints += petsWeight;
                if (currentUserPref.Pets.HasValue && matchPref.Pets.HasValue)
                {
                    double score = currentUserPref.Pets == matchPref.Pets ? 1.0 : 0.0;
                    earnedPoints += score * petsWeight;

                    if (score == 1.0) matchedTraits.Add(currentUserPref.Pets.Value ? "Both love pets" : "Both prefer no pets");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Pets Preference");
                        if (currentUserPref.PetsImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Pets mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Sleep Schedule (Special Ordinal)
                double sleepWeight = (int)currentUserPref.SleepImportance;
                totalPossiblePoints += sleepWeight;
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
                        // One is EarlyBird (1), other is NightOwl (2) -> diff is 1. max diff is 1.
                        int diff = Math.Abs((int)currentUserPref.SleepSchedule - (int)matchPref.SleepSchedule);
                        score = 1.0 - diff; // result is 0.0
                    }

                    earnedPoints += score * sleepWeight;
                    if (score == 1.0) matchedTraits.Add("Compatible Sleep Schedule");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Sleep Schedule");
                        if (currentUserPref.SleepImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Sleep Schedule mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Education Level (Linear Ordinal - Max Diff 3)
                double eduWeight = (int)currentUserPref.EducationImportance;
                totalPossiblePoints += eduWeight;
                if (currentUserPref.EducationLevel != EducationLevel.Unknown && matchPref.EducationLevel != EducationLevel.Unknown)
                {
                    int diff = Math.Abs((int)currentUserPref.EducationLevel - (int)matchPref.EducationLevel);
                    double score = 1.0 - (diff / 3.0); 
                    earnedPoints += score * eduWeight;

                    if (score >= 0.8) matchedTraits.Add("Similar Education Level");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Education Level");
                        if (currentUserPref.EducationImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Education Level mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Field of Study (Strict Categorical)
                double fieldWeight = (int)currentUserPref.FieldOfStudyImportance;
                totalPossiblePoints += fieldWeight;
                if (currentUserPref.FieldOfStudy != FieldOfStudy.Unknown && matchPref.FieldOfStudy != FieldOfStudy.Unknown)
                {
                    double score = currentUserPref.FieldOfStudy == matchPref.FieldOfStudy ? 1.0 : 0.0;
                    earnedPoints += score * fieldWeight;

                    if (score == 1.0) matchedTraits.Add("Same Field of Study");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Field of Study");
                        if (currentUserPref.FieldOfStudyImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Field of Study mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Noise Tolerance (Linear Ordinal - Max Diff 4)
                double noiseWeight = (int)currentUserPref.NoiseToleranceImportance;
                totalPossiblePoints += noiseWeight;
                if (currentUserPref.NoiseTolerance.HasValue && matchPref.NoiseTolerance.HasValue)
                {
                    int diff = Math.Abs(currentUserPref.NoiseTolerance.Value - matchPref.NoiseTolerance.Value);
                    double score = 1.0 - (diff / 4.0);
                    earnedPoints += Math.Max(0, score) * noiseWeight;
                    
                    if (score >= 0.75) matchedTraits.Add("Similar Noise Tolerance");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Noise Tolerance");
                        if (currentUserPref.NoiseToleranceImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Noise Tolerance mismatch");
                            penalty += 40;
                        }
                    }
                }
                
                // Guests Frequency (Linear Ordinal - Max Diff 3)
                double guestsWeight = (int)currentUserPref.GuestsFrequencyImportance;
                totalPossiblePoints += guestsWeight;
                if (currentUserPref.GuestsFrequency != GuestsFrequency.Unknown && matchPref.GuestsFrequency != GuestsFrequency.Unknown)
                {
                    int diff = Math.Abs((int)currentUserPref.GuestsFrequency - (int)matchPref.GuestsFrequency);
                    double score = 1.0 - (diff / 3.0);
                    earnedPoints += score * guestsWeight;
                    
                    if (score >= 0.7) matchedTraits.Add("Similar Guests Preference");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Guests Frequency");
                        if (currentUserPref.GuestsFrequencyImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Guests Frequency mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Sharing Level (Linear Ordinal - Max Diff 2)
                double sharingWeight = (int)currentUserPref.SharingLevelImportance;
                totalPossiblePoints += sharingWeight;
                if (currentUserPref.SharingLevel != SharingLevel.Unknown && matchPref.SharingLevel != SharingLevel.Unknown)
                {
                    int diff = Math.Abs((int)currentUserPref.SharingLevel - (int)matchPref.SharingLevel);
                    double score = 1.0 - (diff / 2.0);
                    earnedPoints += score * sharingWeight;

                    if (score < 0.5)
                    {
                        mismatchedTraits.Add("Sharing Level");
                        if (currentUserPref.SharingLevelImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Sharing Level mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Work Schedule (Strict Categorical)
                double workWeight = (int)currentUserPref.WorkScheduleImportance;
                totalPossiblePoints += workWeight;
                if (currentUserPref.WorkSchedule != WorkSchedule.Unknown && matchPref.WorkSchedule != WorkSchedule.Unknown)
                {
                    double score = currentUserPref.WorkSchedule == matchPref.WorkSchedule ? 1.0 : 0.0;
                    earnedPoints += score * workWeight;

                    if (score == 1.0) matchedTraits.Add("Same Work Schedule");
                    else if (score < 0.5)
                    {
                        mismatchedTraits.Add("Work Schedule");
                        if (currentUserPref.WorkScheduleImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Work Schedule mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Budget Overlap (Direct Proportional)
                double budgetWeight = (int)currentUserPref.BudgetImportance;
                totalPossiblePoints += budgetWeight;
                if (currentUserPref.BudgetRangeMin.HasValue && currentUserPref.BudgetRangeMax.HasValue &&
                    matchPref.BudgetRangeMin.HasValue && matchPref.BudgetRangeMax.HasValue)
                {
                    var overlapStart = Math.Max(currentUserPref.BudgetRangeMin.Value, matchPref.BudgetRangeMin.Value);
                    var overlapEnd = Math.Min(currentUserPref.BudgetRangeMax.Value, matchPref.BudgetRangeMax.Value);
                    
                    if (overlapStart <= overlapEnd)
                    {
                        double currentUserRange = (double)(currentUserPref.BudgetRangeMax.Value - currentUserPref.BudgetRangeMin.Value);
                        if(currentUserRange <= 0) currentUserRange = 1; 
                        double overlapAmount = (double)(overlapEnd - overlapStart);
                        double score = Math.Min(1.0, overlapAmount / currentUserRange); 
                        
                        earnedPoints += score * budgetWeight;
                        if (score >= 0.5) matchedTraits.Add("Compatible Budget");
                        else if (score < 0.5)
                        {
                            mismatchedTraits.Add("Budget");
                            if (currentUserPref.BudgetImportance == PreferenceImportance.Dealbreaker)
                            {
                                dealbreakers.Add("Insufficient Budget overlap");
                                penalty += 40;
                            }
                        }
                    }
                    else
                    {
                        mismatchedTraits.Add("Budget");
                        if (currentUserPref.BudgetImportance == PreferenceImportance.Dealbreaker)
                        {
                            dealbreakers.Add("Budget Mismatch");
                            penalty += 40;
                        }
                    }
                }

                // Calculate final score
                double rawScore = 100 * (earnedPoints / (totalPossiblePoints == 0 ? 1 : totalPossiblePoints));
                
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
                    TopMatchingTraits = matchedTraits.Take(3).ToList(),
                    MismatchedTraits = mismatchedTraits.Take(3).ToList(),
                    DealbreakersFound = dealbreakers
                });
            }

            return matchedResults.OrderByDescending(x => x.CompatibilityScore).Take(k);
        }
    }
}
