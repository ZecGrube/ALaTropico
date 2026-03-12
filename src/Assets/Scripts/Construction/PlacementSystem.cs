using UnityEngine;
using CaudilloBay.World;
using CaudilloBay.AI;

namespace CaudilloBay.Construction
{
    public class PlacementSystem : MonoBehaviour
    {
        public GameObject buildingPrefab;
        public Camera mainCamera;
        public LayerMask terrainLayer;
        public BuilderAI currentBuilder;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }

        private void PlaceBuilding()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, terrainLayer))
            {
                Vector2Int gridPos = TileManager.Instance.WorldToGrid(hit.point);

                if (CanPlaceAt(gridPos))
                {
                    Building buildingComp = buildingPrefab.GetComponent<Building>();
                    if (buildingComp != null && !TechnologyManager.Instance.IsBuildingUnlocked(buildingComp.buildingId))
                    {
                        Debug.LogWarning("Building locked by technology!");
                        return;
                    }

                    Vector3 spawnPos = TileManager.Instance.GridToWorld(gridPos);
                    GameObject newBuildingObj = Instantiate(buildingPrefab, spawnPos, Quaternion.identity);

                    Building newBuilding = newBuildingObj.GetComponent<Building>();
                    if (newBuilding != null)
                    {
                        TileManager.Instance.OccupyTile(gridPos, newBuildingObj);
                        TaskManager.Instance.AddConstructionTask(newBuilding);
                    }
                }
            }
        }

        private bool CanPlaceAt(Vector2Int gridPos)
        {
            // 1. Is it occupied?
            if (TileManager.Instance.IsTileOccupied(gridPos)) return false;

            // 2. Is it water? (Simplification: check height or a specific layer if needed)
            // For now, TileManager doesn't track type, but in a real case we'd query its TileData
            return true;
        }
    }
}
