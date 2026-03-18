using UnityEngine;

namespace CaudilloBay.Economy
{
    public class ResearchVessel : Vehicle
    {
        public float scanRadius = 20f;
        public float scanInterval = 5f;
        private float scanTimer = 0f;

        protected override void Update()
        {
            base.Update();

            scanTimer += Time.deltaTime;
            if (scanTimer >= scanInterval)
            {
                if (World.OffshoreManager.Instance != null)
                {
                    World.OffshoreManager.Instance.ScanArea(new Vector2(transform.position.x, transform.position.z), scanRadius);
                }
                scanTimer = 0f;
            }
        }
    }
}
