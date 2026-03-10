using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.World
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Instance { get; private set; }

        private Dictionary<Vector2Int, GameObject> occupiedTiles = new Dictionary<Vector2Int, GameObject>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.z));
        }

        public Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return new Vector3(gridPosition.x, 0, gridPosition.y);
        }

        public bool IsTileOccupied(Vector2Int gridPosition)
        {
            return occupiedTiles.ContainsKey(gridPosition);
        }

        public void OccupyTile(Vector2Int gridPosition, GameObject building)
        {
            if (!IsTileOccupied(gridPosition))
            {
                occupiedTiles.Add(gridPosition, building);
            }
        }

        public void VacateTile(Vector2Int gridPosition)
        {
            if (IsTileOccupied(gridPosition))
            {
                occupiedTiles.Remove(gridPosition);
            }
        }
    }
}
