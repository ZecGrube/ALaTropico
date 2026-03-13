using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.AI;

namespace CaudilloBay.Construction
{
    public class PlacementSystem : MonoBehaviour
    {
        public BuildingData selectedBuildingData;
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

                if (CanPlaceAt(gridPos, selectedBuildingData))
                {
                    if (!TechnologyManager.Instance.IsBuildingUnlocked(selectedBuildingData.buildingId))
                    {
                        Debug.LogWarning("Building locked by technology!");
                        return;
                    }

                    Vector3 spawnPos = TileManager.Instance.GridToWorld(gridPos);
                    GameObject newBuildingObj = Instantiate(selectedBuildingData.prefab, spawnPos, Quaternion.identity);

                    Building newBuilding = newBuildingObj.GetComponent<Building>();
                    if (newBuilding != null)
                    {
                        newBuilding.data = selectedBuildingData;
                        newBuilding.GridPosition = (gridPos.x, gridPos.y);

                        // Occupy all tiles in footprint
                        for (int x = 0; x < selectedBuildingData.footprint.x; x++)
                        {
                            for (int y = 0; y < selectedBuildingData.footprint.y; y++)
                            {
                                TileManager.Instance.OccupyTile(new Vector2Int(gridPos.x + x, gridPos.y + y), newBuildingObj);
                            }
                        }

                        TaskManager.Instance.AddConstructionTask(newBuilding);
                    }
                }
            }
        }

        private bool CanPlaceAt(Vector2Int gridPos, BuildingData data)
        {
            if (data == null) return false;

            for (int x = 0; x < data.footprint.x; x++)
            {
                for (int y = 0; y < data.footprint.y; y++)
                {
                    Vector2Int pos = new Vector2Int(gridPos.x + x, gridPos.y + y);
                    if (TileManager.Instance.IsTileOccupied(pos)) return false;
                }
            }
            return true;
        }
    }
}
