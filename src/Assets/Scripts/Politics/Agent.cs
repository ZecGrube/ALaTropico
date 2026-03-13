using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class Agent
    {
        public string agentName;
        public bool isUnique; // For bodyguards

        [Range(0, 100)] public float stealth;
        [Range(0, 100)] public float combat;
        [Range(0, 100)] public float charisma;
        [Range(0, 100)] public float tech;

        public bool isOnMission;

        public float GetSuccessChance(GlobalMission mission)
        {
            float skillFactor = 0;
            switch (mission.type)
            {
                case MissionType.Espionage: skillFactor = (stealth + tech) / 2f; break;
                case MissionType.Trade: skillFactor = charisma; break;
                case MissionType.Propaganda: skillFactor = (charisma + stealth) / 2f; break;
                case MissionType.MilitaryAid: skillFactor = combat; break;
            }

            return Mathf.Clamp(skillFactor - (mission.baseDifficulty * 50f), 5f, 95f);
        }
    }
}
