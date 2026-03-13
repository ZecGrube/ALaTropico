using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [CreateAssetMenu(fileName = "New Decree", menuName = "Caudillo Bay/Decree")]
    public class Decree : ScriptableObject
    {
        public string decreeName;
        [TextArea]
        public string description;
        public int mandateCost;

        public List<FactionEffect> loyaltyEffects;
        public float taxModifier;
        public float wageModifier;
    }

    [System.Serializable]
    public struct FactionEffect
    {
        public FactionType faction;
        public float effect;
    }
}
