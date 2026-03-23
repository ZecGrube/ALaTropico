using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Politics
{
    public class OrganizationManager : MonoBehaviour
    {
        public static OrganizationManager Instance { get; private set; }

        public List<InternationalOrganization> activeMemberships = new List<InternationalOrganization>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void TryJoin(InternationalOrganization org)
        {
            if (activeMemberships.Contains(org)) return;

            if (LegitimacySystem.Instance != null && LegitimacySystem.Instance.currentLegitimacy >= org.legitimacyThreshold)
            {
                activeMemberships.Add(org);
                ApplyBonuses(org);
                Debug.Log($"Joined International Organization: {org.orgName}");
            }
            else
            {
                Debug.LogWarning($"Failed to join {org.orgName}: Legitimacy too low.");
            }
        }

        public void ApplyBonuses(InternationalOrganization org)
        {
            if (Core.ModifierManager.Instance != null)
            {
                foreach (var mod in org.membershipBonuses)
                {
                    Core.ModifierManager.Instance.AddModifier(mod);
                }
            }
        }

        public void ProcessYearlyDues()
        {
            float total = 0;
            foreach (var org in activeMemberships) total += org.yearlyDues;

            if (Economy.EconomyManager.Instance != null)
            {
                Economy.EconomyManager.Instance.AddFunds(-total);
                Debug.Log($"Paid ${total} in international organization dues.");
            }
        }
    }
}
