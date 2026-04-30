using UnityEngine;

namespace CaudilloBay.Ecology
{
    [CreateAssetMenu(fileName = "New Species", menuName = "Caudillo Bay/Ecology/Animal Species")]
    public class AnimalSpecies : ScriptableObject
    {
        public string speciesId;
        public string speciesName;
        [TextArea] public string description;
        public Sprite icon;

        public float conservationStatus = 100f; // 0 (Extinct) to 100 (Abundant)
        public float trophyValue = 50f;
        public float ecoTourismBonus = 5f;
        public float birthRate = 0.05f;
        public float deathRate = 0.02f;
    }
}
