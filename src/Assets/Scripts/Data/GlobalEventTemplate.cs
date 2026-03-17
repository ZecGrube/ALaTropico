using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum GlobalEventType { Economic, Political, Disaster, Tech, Military }

    [CreateAssetMenu(fileName = "NewGlobalEvent", menuName = "CaudilloBay/GlobalEventTemplate")]
    public class GlobalEventTemplate : ScriptableObject
    {
        public string eventId;
        public string titleTemplate;
        [TextArea] public string descriptionTemplate;
        public GlobalEventType type;

        public float baseSpawnChance = 0.1f;
        public float duration = 0f; // 0 for instant, >0 for months

        public List<GlobalEventOption> options;
    }

    [System.Serializable]
    public class GlobalEventOption
    {
        public string optionText;
        public List<GlobalEventEffect> effects;
        public ResourceCost cost;
        public float successChance = 1.0f;
    }

    [System.Serializable]
    public class GlobalEventEffect
    {
        public enum EffectType { ChangeRelations, AddFunds, SpawnRefugees, AddTech, TriggerSanction }
        public EffectType type;
        public float value;
        public string data; // For techId or other string refs
    }
}
