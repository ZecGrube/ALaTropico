using UnityEngine;

namespace CaudilloBay.Systems.Nuclear
{
    [System.Serializable]
    public class NuclearWarhead
    {
        public string warheadId;
        public string warheadName;
        public float yieldMT; // Yield in Megatons
        public float reliability = 0.8f; // Chance to successfully detonate
        public float productionCost = 5000f;
    }

    public enum DeliveryMethod { ICBM, Bomber, Submarine }

    [System.Serializable]
    public abstract class DeliverySystem
    {
        public string systemName;
        public DeliveryMethod method;
        public float range;
        public float interceptionChance; // Base chance to be intercepted by enemy AA/ABM
        public float maintenanceCost;

        public NuclearWarhead loadedWarhead;

        public bool IsLoaded => loadedWarhead != null;
    }

    [System.Serializable]
    public class ICBM : DeliverySystem
    {
        public ICBM()
        {
            method = DeliveryMethod.ICBM;
            range = 10000f; // Global reach
            interceptionChance = 0.2f;
        }
    }

    [System.Serializable]
    public class NuclearBomber : DeliverySystem
    {
        public NuclearBomber()
        {
            method = DeliveryMethod.Bomber;
            range = 5000f;
            interceptionChance = 0.5f;
        }
    }

    [System.Serializable]
    public class SSBN : DeliverySystem
    {
        public SSBN()
        {
            method = DeliveryMethod.Submarine;
            range = 8000f;
            interceptionChance = 0.3f;
        }
    }
}
