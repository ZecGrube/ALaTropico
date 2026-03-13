using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.World;

namespace CaudilloBay.Politics
{
    public enum MissionType { Trade, Propaganda, Espionage, MilitaryAid }

    [CreateAssetMenu(fileName = "New Global Mission", menuName = "Caudillo Bay/Global Mission")]
    public class GlobalMission : ScriptableObject
    {
        public string title;
        [TextArea]
        public string description;
        public MissionType type;

        public float duration; // in months/seconds
        public float baseDifficulty; // 0 to 1

        public float rewardRelations;
        public List<ResourceCost> requiredResources;
    }
}
