using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [CreateAssetMenu(fileName = "New Personal Mission", menuName = "Caudillo Bay/Personal Mission")]
    public class PersonalMission : ScriptableObject
    {
        public string title;
        [TextArea]
        public string description;
        public GlobalMission missionTemplate;

        public float skillReward; // e.g. +5 stealth
        public string rewardText;
    }
}
