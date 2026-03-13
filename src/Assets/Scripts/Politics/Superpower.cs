using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [CreateAssetMenu(fileName = "New Superpower", menuName = "Caudillo Bay/Superpower")]
    public class Superpower : ScriptableObject
    {
        public string powerName;
        public Sprite flag;
        public float relations = 0f; // -100 to 100

        public List<GlobalMission> missionTemplates = new List<GlobalMission>();
    }
}
