using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public enum SuperpowerType
    {
        USA,
        USSR,
        China,
        EU,
        Independent
    }

    [CreateAssetMenu(fileName = "New Superpower", menuName = "Caudillo Bay/Superpower")]
    public class Superpower : ScriptableObject
    {
        public SuperpowerType type;
        public string powerName;
        public Sprite flag;
        public float relations = 0f; // -100 to 100

        public List<GlobalMission> missionTemplates = new List<GlobalMission>();
    }
}
