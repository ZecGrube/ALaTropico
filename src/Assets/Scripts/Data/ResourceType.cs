using UnityEngine;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Caudillo Bay/Resource Type")]
    public class ResourceType : ScriptableObject
    {
        public string resourceId;
        public string resourceName;
        public Sprite icon;
        public float unitWeight = 1.0f;
        public float baseValue = 10.0f;
    }
}
