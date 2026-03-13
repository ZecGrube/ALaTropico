using UnityEngine;

namespace CaudilloBay.World
{
    public class Airfield : Building
    {
        public float airPowerBonus = 15f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            var military = CaudilloBay.Politics.MilitaryManager.Instance;
            if (military != null) military.AddAirPower(airPowerBonus);
        }

        private void OnDestroy()
        {
            var military = CaudilloBay.Politics.MilitaryManager.Instance;
            if (military != null) military.RemoveAirPower(airPowerBonus);
        }
    }
}
