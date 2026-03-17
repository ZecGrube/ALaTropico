using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.AI
{
    public class PensionManager : MonoBehaviour
    {
        public static PensionManager Instance { get; private set; }

        public float retirementAge = 65f;
        public float monthlyPensionAmount = 20f;
        public float socialSecurityTaxRate = 0.05f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessPensions(List<Citizen> population)
        {
            float totalPensionOutflow = 0f;
            int retiredCount = 0;

            foreach (var citizen in population)
            {
                if (citizen.age >= retirementAge)
                {
                    citizen.isRetired = true;
                    citizen.workplace = null; // Retirees don't work
                    citizen.personalWealth += monthlyPensionAmount;
                    totalPensionOutflow += monthlyPensionAmount;
                    retiredCount++;
                }
            }

            if (Economy.EconomyManager.Instance != null)
            {
                Economy.EconomyManager.Instance.AddFunds(-totalPensionOutflow);
            }

            Debug.Log($"Pension Update: {retiredCount} retirees received ${totalPensionOutflow} total.");
        }
    }
}
