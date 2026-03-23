using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Politics
{
    public enum AllianceType { Military, Trade, Defensive }

    [System.Serializable]
    public class Alliance
    {
        public string allianceName;
        public List<NeighborState> neighborMembers = new List<NeighborState>();
        public AllianceType type;
        public float cohesion = 50f;

        public Alliance(string name, AllianceType t)
        {
            allianceName = name;
            type = t;
        }

        public void UpdateCohesion()
        {
            float totalRelations = 0;
            foreach (var n in neighborMembers) totalRelations += n.relations;

            if (neighborMembers.Count > 0)
                cohesion = Mathf.Lerp(cohesion, totalRelations / neighborMembers.Count, 0.1f);
        }
    }

    public class AllianceManager : MonoBehaviour
    {
        public static AllianceManager Instance { get; private set; }

        public List<Alliance> activeAlliances = new List<Alliance>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CreateAlliance(string name, AllianceType type, NeighborState partner)
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
