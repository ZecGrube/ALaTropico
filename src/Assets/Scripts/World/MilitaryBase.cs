using UnityEngine;
using CaudilloBay.Politics;

namespace CaudilloBay.World
{
    public class MilitaryBase : Building
    {
        public float trainingBonusPerMonth = 0.5f;

        private void Update()
        {
            if (IsFunctional() && MilitaryManager.Instance != null)
            {
                MilitaryManager.Instance.trainingLevel = Mathf.Clamp(MilitaryManager.Instance.trainingLevel + (trainingBonusPerMonth / 60f) * Time.deltaTime, 0, 100);
                MilitaryManager.Instance.UpdateMilitaryStrength();
            }
        }
    }
}
