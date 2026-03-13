using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class CoupManager : MonoBehaviour
    {
        public static CoupManager Instance { get; private set; }

        public bool isCoupActive = false;
        public float coupPower = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CheckCoupConditions()
        {
            if (isCoupActive) return;

            float legitimacy = LegitimacySystem.Instance.currentLegitimacy;

            // Basic trigger: low legitimacy or extremely unhappy factions
            if (legitimacy < 30f)
            {
                float chance = (30f - legitimacy) * 2f; // Up to 60% chance if legitimacy is 0
                if (UnityEngine.Random.Range(0, 100) < chance)
                {
                    StartCoup();
                }
            }
        }

        private void StartCoup()
        {
            isCoupActive = true;
            coupPower = UnityEngine.Random.Range(30f, 70f);

            // Check for international superpower intervention
            if (GlobalMapManager.Instance != null)
            {
                foreach (var sp in GlobalMapManager.Instance.superpowers)
                {
                    if (sp.relations < -50f)
                    {
                        Debug.Log($"{sp.powerName} is supporting the putschists!");
                        coupPower += 20f;
                    }
                }
            }

            Debug.Log("A Coup has started! Legitimacy is too low.");
            // Notify UI
        }

        public void ResolveCoup(bool suppress)
        {
            if (!isCoupActive) return;

            if (suppress)
            {
                float governmentStrength = MilitaryManager.Instance.totalMilitaryStrength * (MilitaryManager.Instance.armyLoyalty / 100f);
                if (governmentStrength >= coupPower)
                {
                    Debug.Log("Coup Suppressed by military force.");
                    LegitimacySystem.Instance.ModifyLegitimacy(10f);
                }
                else
                {
                    Debug.Log("Military failed to suppress the coup. Overthrown!");
                    // Trigger Game Over
                }
            }
            else
            {
                // Negotiate
                Debug.Log("Negotiated with putschists. Power retained but legitimacy lost.");
                LegitimacySystem.Instance.ModifyLegitimacy(-30f);
            }

            isCoupActive = false;
        }
    }
}
