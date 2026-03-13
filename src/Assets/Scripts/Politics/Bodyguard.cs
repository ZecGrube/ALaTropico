using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class Bodyguard : Agent
    {
        public string backstory;
        public List<PersonalMission> personalMissions = new List<PersonalMission>();
        public int currentMissionIndex = 0;

        public bool HasAvailableMission() => currentMissionIndex < personalMissions.Count;
        public PersonalMission GetNextMission() => HasAvailableMission() ? personalMissions[currentMissionIndex] : null;
    }
}
