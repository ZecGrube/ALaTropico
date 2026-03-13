using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Core;
using CaudilloBay.Politics;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New Invasion Event", menuName = "CaudilloBay/Invasion Event")]
    public class InvasionEvent : GameEvent
    {
        public float invasionForce = 500f;
        public int durationMonths = 3;

        public void ResolveInvasion()
        {
            if (MilitaryManager.Instance == null) return;

            if (MilitaryManager.Instance.totalMilitaryStrength >= invasionForce)
            {
                Debug.Log("Invasion repelled! Military Victory.");
            }
            else
            {
                Debug.Log("Invasion successful. Defeat.");
                // Apply damage to buildings, lose Legitimacy
            }
        }
    }
}
