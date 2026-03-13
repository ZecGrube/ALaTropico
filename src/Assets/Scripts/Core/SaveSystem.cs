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
        public class GameSaveData
        {
            public float legitimacy;
            public int mandate;
            public float pollution;
            public List<FactionSaveData> factions = new List<FactionSaveData>();
            // Resource stockpiles could be added here as well
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
                data.pollution = StatsManager.Instance.globalPollution;

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
                StatsManager.Instance.globalPollution = data.pollution;

            Debug.Log("Game loaded successfully.");
        }
    }
}
