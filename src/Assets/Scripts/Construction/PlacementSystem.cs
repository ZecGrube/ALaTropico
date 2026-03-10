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
                if (!TileManager.Instance.IsTileOccupied(gridPos))
                {
                    Vector3 spawnPos = TileManager.Instance.GridToWorld(gridPos);
                    GameObject newBuilding = Instantiate(buildingPrefab, spawnPos, Quaternion.identity);
                    TileManager.Instance.OccupyTile(gridPos, newBuilding);

                    if (currentBuilder != null)
                    {
                        currentBuilder.SetTargetConstruction(newBuilding.transform);
                    }
                }
            }
        }
    }
}
