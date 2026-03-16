using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.Politics;
using CaudilloBay.World;

namespace CaudilloBay.Core
{
    public class PersonalityCultManager : MonoBehaviour
    {
        public static PersonalityCultManager Instance { get; private set; }

        [Header("State")]
        [Range(0, 100)] public float cultLevel = 0f;

        private List<CultBuilding> cultAssets = new List<CultBuilding>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddCultPoints(float amount)
        {
            cultLevel = Mathf.Clamp(cultLevel + amount, 0, 100);
        }

        public void RegisterCultAsset(CultBuilding asset)
        {
            if (!cultAssets.Contains(asset))
            {
                cultAssets.Add(asset);
                UpdateCultLevel();
            }
        }

        public void UnregisterCultAsset(CultBuilding asset)
        {
            if (cultAssets.Contains(asset))
            {
                cultAssets.Remove(asset);
                UpdateCultLevel();
            }
        }

        public void UpdateCultLevel()
        {
            float total = 0f;
            foreach (var asset in cultAssets)
            {
                if (asset.IsFunctional())
                {
                    total += asset.cultBonus;
                }
            }
            // Base cult comes from buildings, but can be augmented by events
            cultLevel = Mathf.Clamp(total, 0, 100);
        }

        public void ProcessMonthlyUpdate()
        {
            UpdateCultLevel();

            if (FactionManager.Instance != null)
            {
                // Nationalists love the cult
                ModifyLoyalty(FactionType.Nationalists, cultLevel * 0.1f);

                // Religious faction might also like it if aligned
                ModifyLoyalty(FactionType.Religious, cultLevel * 0.05f);

                // Intellectuals/Liberals (if added) would dislike it
            }

            // High cult boosts legitimacy
            if (LegitimacySystem.Instance != null)
            {
                LegitimacySystem.Instance.currentLegitimacy += cultLevel * 0.02f;
            }
        }

        private void ModifyLoyalty(FactionType type, float delta)
        {
            var faction = FactionManager.Instance.factions.Find(f => f.type == type);
            if (faction != null)
            {
                faction.loyalty = Mathf.Clamp(faction.loyalty + delta, 0, 100);
            }
        }
    }
}
