using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Core
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }

        public List<Achievement> allAchievements = new List<Achievement>();
        public HashSet<string> unlockedAchievements = new HashSet<string>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CheckAchievement(AchievementType type, float currentValue)
        {
            foreach (var ach in allAchievements)
            {
                if (ach.type == type && !unlockedAchievements.Contains(ach.achievementId))
                {
                    if (currentValue >= ach.targetValue)
                    {
                        UnlockAchievement(ach);
                    }
                }
            }
        }

        private void UnlockAchievement(Achievement ach)
        {
            unlockedAchievements.Add(ach.achievementId);
            Debug.Log($"Achievement Unlocked: {LocalizationManager.Instance.GetText(ach.titleKey)}");

            // Trigger UI notification
            if (SteamManager.Instance != null)
                SteamManager.Instance.UnlockAchievement(ach.achievementId);
        }
    }
}
