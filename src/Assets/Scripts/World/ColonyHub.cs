using UnityEngine;
using CaudilloBay.Data;

namespace CaudilloBay.World
{
    public class ColonyHub : Building
    {
        public int currentLevel = 1;
        public float colonyBonusMultiplier = 1.1f;

        public override bool IsFunctional()
        {
            return base.IsFunctional() && currentHealth > 50f;
        }

        public void Upgrade()
        {
            currentLevel++;
            colonyBonusMultiplier += 0.1f;
            Debug.Log($"Colony Hub upgraded to level {currentLevel}");
        }
    }
}
