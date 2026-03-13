using UnityEngine;

namespace CaudilloBay.World
{
    public class MilitaryBase : Building
    {
        public float trainingBonus = 12f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            var military = CaudilloBay.Politics.MilitaryManager.Instance;
            if (military != null)
            {
                military.ImproveTraining(trainingBonus);
            }
        }

        private void OnDestroy()
        {
            var military = CaudilloBay.Politics.MilitaryManager.Instance;
            if (military != null)
            {
                military.ReduceTraining(trainingBonus);
            }
        }
    }
}
