using UnityEngine;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "BuildingSettings", menuName = "Caudillo Bay/Building Settings")]
    public class BuildingSettings : ScriptableObject
    {
        public string id;
        public string displayName;
        public int cost;
        public Vector2Int footprint = new Vector2Int(1, 1);
        public GameObject prefab;
    }
}
