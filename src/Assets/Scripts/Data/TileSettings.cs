using UnityEngine;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "TileSettings", menuName = "Caudillo Bay/Tile Settings")]
    public class TileSettings : ScriptableObject
    {
        public TerrainType type;
        public GameObject prefab;
        public bool isWalkable = true;
        public bool isBuildable = true;
    }

    public enum TerrainType
    {
        Water,
        Sand,
        Grass,
        Mountain
    }
}
