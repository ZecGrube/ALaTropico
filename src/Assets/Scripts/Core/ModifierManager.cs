using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.Core
{
    public class ModifierManager : MonoBehaviour
    {
        public static ModifierManager Instance { get; private set; }

        [System.Serializable]
        public class ActiveModifier
        {
            public ModifierData data;
            public int remainingMonths;
        }

        private List<ActiveModifier> activeModifiers = new List<ActiveModifier>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddModifier(ModifierData data)
        {
            activeModifiers.Add(new ActiveModifier { data = data, remainingMonths = data.durationMonths });
            Debug.Log($"Modifier Added: {data.displayName} for {data.durationMonths} months.");
        }

        public void ProcessMonthlyTick()
        {
            for (int i = activeModifiers.Count - 1; i >= 0; i--)
            {
                activeModifiers[i].remainingMonths--;
                if (activeModifiers[i].remainingMonths <= 0)
                {
                    Debug.Log($"Modifier Expired: {activeModifiers[i].data.displayName}");
                    activeModifiers.RemoveAt(i);
                }
            }
        }

        public float GetTotalModifier(ModifierType type)
        {
            float total = 0f;
            foreach (var mod in activeModifiers)
            {
                if (mod.data.type == type) total += mod.data.value;
            }
            return total;
        }

        public List<ActiveModifier> GetActiveModifiers() => activeModifiers;

        public void LoadModifiers(List<ActiveModifier> loaded)
        {
            activeModifiers = loaded;
        }
    }
}
