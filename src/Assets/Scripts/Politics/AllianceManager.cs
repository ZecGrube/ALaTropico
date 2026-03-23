using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class AllianceManager : MonoBehaviour
    {
        public static AllianceManager Instance { get; private set; }

        public List<Alliance> activeAlliances = new List<Alliance>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CreateAlliance(string name, AllianceType type, Data.NeighborState partner)
        {
            if (partner.relations < 60)
            {
                Debug.LogWarning($"{partner.stateName} rejected the alliance proposal.");
                return;
            }

            Alliance newAlliance = new Alliance(name, type);
            newAlliance.neighborMembers.Add(partner);
            activeAlliances.Add(newAlliance);
            Debug.Log($"New Alliance formed: {name} with {partner.stateName}");
        }

        public void MonthlyUpdate()
        {
            foreach (var alliance in activeAlliances)
            {
                alliance.UpdateCohesion();
            }
        }
    }
}
