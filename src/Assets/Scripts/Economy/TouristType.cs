using UnityEngine;

namespace CaudilloBay.Economy
{
    public enum TouristCategory
    {
        Budget,
        Luxury,
        Eco,
        Business
    }

    [System.Serializable]
    public class TouristType
    {
        public TouristCategory category;
        public float budget;
        public float expectations; // 0-100
        public float stayDuration;

        // Preferred attractions
        public float luxuryWeight;
        public float natureWeight;
        public float entertainmentWeight;
    }
}
