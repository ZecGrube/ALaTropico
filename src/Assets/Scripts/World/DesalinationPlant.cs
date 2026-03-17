using UnityEngine;

namespace CaudilloBay.World
{
    public class DesalinationPlant : ProducerBuilding
    {
        [Header("Desalination")]
        public float waterPurity = 95f;

        public override void ProduceCycle()
        {
            base.ProduceCycle();
            // During droughts, Desalination Plant output is even more critical
            if (Core.ClimateManager.Instance != null && Core.ClimateManager.Instance.temperatureIncrease > 2f)
            {
                Debug.Log("Desalination Plant running at maximum capacity due to high heat.");
            }
        }
    }
}
