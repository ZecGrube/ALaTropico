using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Systems
{
    public enum UtilityNodeType { Producer, Consumer, Storage, Junction }
    public enum UtilityType { Power, Water }

    public abstract class UtilityNode : MonoBehaviour
    {
        public UtilityNodeType nodeType;
        public UtilityType utilityType;

        public float capacity = 0f;
        public float demand = 0f;
        public float currentStorage = 0f;

        public List<UtilityConnection> connections = new List<UtilityConnection>();

        [HideInInspector]
        public bool isSatisfied = false;
        [HideInInspector]
        public int networkId = -1;

        public virtual void Register()
        {
            if (utilityType == UtilityType.Power) PowerGridManager.Instance.RegisterNode(this);
            else WaterNetworkManager.Instance.RegisterNode(this);
        }

        public virtual void Unregister()
        {
            if (utilityType == UtilityType.Power) PowerGridManager.Instance.UnregisterNode(this);
            else WaterNetworkManager.Instance.UnregisterNode(this);
        }

        private void OnDestroy()
        {
            Unregister();
        }
    }
}
