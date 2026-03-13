using UnityEngine;

namespace CaudilloBay.World
{
    public class CoastalDefense : Building
    {
        public float defenseBonus = 20f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            var military = CaudilloBay.Politics.MilitaryManager.Instance;
            if (military != null) military.AddCoastalDefense(defenseBonus);
        }

        private void OnDestroy()
        {
            var military = CaudilloBay.Politics.MilitaryManager.Instance;
            if (military != null) military.RemoveCoastalDefense(defenseBonus);
        }
    }
}
