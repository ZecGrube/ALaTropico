using UnityEngine;

namespace CaudilloBay.World
{
    public class SeaWall : Building
    {
        public float waveProtectionRadius = 10f;
        public float structuralReinforcement = 50f;

        protected override void Start()
        {
            base.Start();
            Debug.Log("SeaWall constructed. Coastal buildings nearby are now protected from storm surges.");
        }
    }
}
