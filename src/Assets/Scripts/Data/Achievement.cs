using UnityEngine;

namespace CaudilloBay.Data
{
    public enum AchievementType { BuildBuilding, ReachPopulation, AccumulateWealth, SurviveCoup }

    [CreateAssetMenu(fileName = "New Achievement", menuName = "Caudillo Bay/Achievement")]
    public class Achievement : ScriptableObject
    {
        public string achievementId;
        public string titleKey;
        public string descriptionKey;
        public Sprite icon;
        public AchievementType type;
        public float targetValue;
    }
}
