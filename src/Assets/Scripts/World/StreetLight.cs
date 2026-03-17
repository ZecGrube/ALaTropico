using UnityEngine;
using CaudilloBay.AI;

namespace CaudilloBay.World
{
    public class StreetLight : Building
    {
        public float lightRadius = 15f;

        private void Update()
        {
            if (IsFunctional())
            {
                ApplyLightingEffect();
            }
        }

        private void ApplyLightingEffect()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, lightRadius);
            foreach (var hit in hits)
            {
                Citizen citizen = hit.GetComponent<Citizen>();
                if (citizen != null)
                {
                    // Provide a 'Lighted' bonus to happiness/safety (abstractly)
                    citizen.satisfaction = Mathf.Min(citizen.satisfaction + 0.01f, 100f);
                }
            }
        }
    }
}
