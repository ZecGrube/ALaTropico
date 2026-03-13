using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [CreateAssetMenu(fileName = "New Political Event", menuName = "Caudillo Bay/Political Event")]
    public class PoliticalEvent : ScriptableObject
    {
        public string eventTitle;
        [TextArea]
        public string eventDescription;
        public float baseChance = 0.1f;
        public List<FactionEffect> loyaltyEffects;
        public float duration = 0f; // 0 for instant
    }
}
