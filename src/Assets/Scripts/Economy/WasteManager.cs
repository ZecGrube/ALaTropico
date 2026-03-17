using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;

namespace CaudilloBay.Economy
{
    public class WasteManager : MonoBehaviour
    {
        public static WasteManager Instance { get; private set; }

        public float totalGarbageOnIsland = 0f;
        public float collectionEfficiency = 0.8f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessMonthlyWaste()
        {
            float generated = 0f;
            foreach (var b in StatsManager.Instance.GetTrackedBuildings())
            {
                if (b.IsFunctional())
                {
                    b.garbageAccumulated += b.garbageOutput;
                    generated += b.garbageOutput;
                }
            }
            totalGarbageOnIsland += generated;
            Debug.Log($"Monthly Waste Generated: {generated}. Total island garbage: {totalGarbageOnIsland}");
        }

        public void CollectGarbage(Building source, float amount)
        {
            float collected = Mathf.Min(source.garbageAccumulated, amount);
            source.garbageAccumulated -= collected;
            totalGarbageOnIsland -= collected;
        }
    }
}
