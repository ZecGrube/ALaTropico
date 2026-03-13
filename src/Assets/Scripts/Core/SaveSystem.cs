using UnityEngine;
using System.Collections.Generic;
using System.IO;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.Core
{
    public class SaveSystem : MonoBehaviour
    {
        [System.Serializable]
        public class FactionSaveData
        {
            public string factionName;
            public float loyalty;
            public float satisfaction;
        }

        [System.Serializable]
        public class ResourceSaveData
        {
            public string resourceId;
            public float amount;
        }

        [System.Serializable]
        public class GameSaveData
        {
            public float legitimacy;
            public int mandate;
            public float pollution;
            public List<FactionSaveData> factions = new List<FactionSaveData>();
            public List<ResourceSaveData> resources = new List<ResourceSaveData>();
            public List<string> researchedTechs = new List<string>();
            public List<BodyguardSaveData> bodyguards = new List<BodyguardSaveData>();
        }

        public void SaveGame(string fileName = "savegame.json")
        {
            GameSaveData data = new GameSaveData();

            // Populate data from managers
            if (LegitimacySystem.Instance != null)
                data.legitimacy = LegitimacySystem.Instance.currentLegitimacy;

            if (FactionManager.Instance != null)
            {
                data.mandate = FactionManager.Instance.currentMandate;
                foreach (var f in FactionManager.Instance.factions)
                {
                    data.factions.Add(new FactionSaveData {
                        factionName = f.displayName,
                        loyalty = f.loyalty,
                        satisfaction = f.needsSatisfaction
                    });
                }
            }

            if (StatsManager.Instance != null)
            {
                data.pollution = StatsManager.Instance.globalPollution;
                foreach (var entry in StatsManager.Instance.globalStockpiles)
                {
                    data.resources.Add(new ResourceSaveData { resourceId = entry.Key, amount = entry.Value });
                }
            }

            if (TechnologyManager.Instance != null)
            {
                data.researchedTechs = TechnologyManager.Instance.GetResearchedTechIds();
            }

            if (GlobalMapManager.Instance != null)
            {
                data.bodyguards = GlobalMapManager.Instance.GetBodyguardData();
            }

            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);

            Debug.Log($"Game saved to {path}");
        }

        public void LoadGame(string fileName = "savegame.json")
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning("Save file not found!");
                return;
            }

            string json = File.ReadAllText(path);
            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            // Apply data to managers
            if (LegitimacySystem.Instance != null)
                LegitimacySystem.Instance.currentLegitimacy = data.legitimacy;

            if (FactionManager.Instance != null)
            {
                FactionManager.Instance.currentMandate = data.mandate;
                foreach (var fs in data.factions)
                {
                    var faction = FactionManager.Instance.factions.Find(f => f.displayName == fs.factionName);
                    if (faction != null)
                    {
                        faction.loyalty = fs.loyalty;
                        faction.needsSatisfaction = fs.satisfaction;
                    }
                }
            }

            if (StatsManager.Instance != null)
            {
                StatsManager.Instance.globalPollution = data.pollution;
                StatsManager.Instance.globalStockpiles.Clear();
                foreach (var rs in data.resources)
                {
                    StatsManager.Instance.globalStockpiles.Add(rs.resourceId, rs.amount);
                }
            }

            if (TechnologyManager.Instance != null)
            {
                TechnologyManager.Instance.LoadResearchedTechs(data.researchedTechs);
            }

            if (GlobalMapManager.Instance != null)
            {
                GlobalMapManager.Instance.LoadBodyguardData(data.bodyguards);
            }

            Debug.Log("Game loaded successfully.");
        }
    }
}
