using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.World
{
    public class IslandGenerator : MonoBehaviour
    {
        [Header("Settings")]
        public int width = 50;
        public int height = 50;
        public float scale = 10f;
        public float waterThreshold = 0.3f;
        public float sandThreshold = 0.4f;

        [Header("Prefabs")]
        public GameObject grassPrefab;
        public GameObject sandPrefab;
        public GameObject waterPrefab;

        private Transform islandParent;

        public void SetupFromSandbox(SandboxSettings settings)
        {
            width = settings.GetWidth();
            height = settings.GetHeight();
            // Scale and thresholds could also be influenced by richness/difficulty
        }

        public void SetupFromMission(CampaignMission mission)
        {
            width = mission.mapWidth;
            height = mission.mapHeight;
            scale = mission.mapScale;
        }

        public void GenerateIsland()
        {
            if (islandParent != null)
            {
                Destroy(islandParent.gameObject);
            }

            islandParent = new GameObject("Island").transform;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    float xCoord = (float)x / width * scale;
                    float zCoord = (float)z / height * scale;
                    float noise = Mathf.PerlinNoise(xCoord, zCoord);

                    GameObject prefab = GetPrefabForNoise(noise);
                    if (prefab != null)
                    {
                        Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity, islandParent);
                    }
                }
            }

            // Bake NavMesh for agents
            var navSurface = GetComponent<Unity.AI.Navigation.NavMeshSurface>();
            if (navSurface != null)
            {
                navSurface.BuildNavMesh();
            }
        }

        private GameObject GetPrefabForNoise(float noise)
        {
            if (noise < waterThreshold) return waterPrefab;
            if (noise < sandThreshold) return sandPrefab;
            return grassPrefab;
        }

        private void Start()
        {
            // Usually triggered by GameStateManager now, but keep as fallback
            if (islandParent == null) GenerateIsland();
        }
    }
}
