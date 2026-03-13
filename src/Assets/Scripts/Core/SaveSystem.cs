using UnityEngine;
using System.Collections.Generic;
using System.IO;
using CaudilloBay.World;
using CaudilloBay.Politics;
using CaudilloBay.Data;

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
        public class RelationSaveData
        {
            public FactionType factionA;
            public FactionType factionB;
            public float value;
        }

        [System.Serializable]
        public class InventorySaveData
        {
            public string resourceId;
            public float amount;
        }

        [System.Serializable]
        public class BuildingSaveData
        {
            public string buildingId;
            public int posX;
            public int posZ;
            public float health;
            public List<InventorySaveData> inventory = new List<InventorySaveData>();
        }

        [System.Serializable]
        public class ModifierSaveData
        {
            public Data.ModifierData data;
            public int remainingMonths;
        }

        [System.Serializable]
        public class HeirSupportSaveData
        {
            public FactionType faction;
            public float support;
        }

        [System.Serializable]
        public class HeirSaveData
        {
            public string heirName;
            public int age;
            public Politics.HeirGender gender;
            public float charisma;
            public float cruelty;
            public float intelligence;
            public float military;
            public float loyaltyToRuler;
            public float jealousy;
            public bool isAlive;
            public List<HeirSupportSaveData> factionSupport = new List<HeirSupportSaveData>();
            public List<Politics.SecretTrait> secretTraits = new List<Politics.SecretTrait>();
        }

        [System.Serializable]
        public class SaveMetadata
        {
            public string fileName;
            public string islandName;
            public string date;
        }

        [System.Serializable]
        public class ObjectiveSaveData
        {
            public string description;
            public float currentValue;
            public bool isComplete;
        }

        [System.Serializable]
        public class GameSaveData
        {
            public GameMode mode;
            public string missionId;
            public SandboxSettings sandboxSettings;
            public float legitimacy;
            public int mandate;
            public float pollution;
            public List<FactionSaveData> factions = new List<FactionSaveData>();
            public List<ResourceSaveData> resources = new List<ResourceSaveData>();
            public List<string> researchedTechs = new List<string>();
            public List<BodyguardSaveData> bodyguards = new List<BodyguardSaveData>();
            public List<RelationSaveData> relations = new List<RelationSaveData>();
            public List<ObjectiveSaveData> missionObjectives = new List<ObjectiveSaveData>();
            public List<BuildingSaveData> buildings = new List<BuildingSaveData>();
            public List<string> completedTutorialSteps = new List<string>();
            public List<string> unlockedAchievements = new List<string>();
            public List<ModifierSaveData> activeModifiers = new List<ModifierSaveData>();
            public List<Politics.SuperpowerType> alliances = new List<Politics.SuperpowerType>();
            public float crimeRate;
            public float educationLevel;
            public float healthLevel;
            public float armyStrength;
            public float trainingLevel;
            public float armyLoyalty;
            public float cultureLevel;
            public float corruptionLevel;
            public float blackMarketMoney;
            public HeirSaveData rulerData;
            public List<HeirSaveData> heirs = new List<HeirSaveData>();
        }

        public void SaveGame(string fileName = "savegame.json")
        {
            GameSaveData data = new GameSaveData();

            if (GameStateManager.Instance != null)
            {
                data.mode = GameStateManager.Instance.currentMode;
                data.sandboxSettings = GameStateManager.Instance.sandboxSettings;
                if (GameStateManager.Instance.activeMission != null)
                    data.missionId = GameStateManager.Instance.activeMission.missionId;
            }

            if (CampaignManager.Instance != null && data.mode == GameMode.Campaign)
            {
                foreach (var obj in CampaignManager.Instance.activeObjectives)
                {
                    data.missionObjectives.Add(new ObjectiveSaveData {
                        description = obj.data.description,
                        currentValue = obj.currentValue,
                        isComplete = obj.isComplete
                    });
                }
            }

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

            if (FactionManager.Instance != null)
            {
                data.relations = FactionManager.Instance.GetRelationSaveData();
            }

            if (UI.TutorialManager.Instance != null)
            {
                foreach (var step in UI.TutorialManager.Instance.completedSteps) data.completedTutorialSteps.Add(step);
            }

            if (AchievementManager.Instance != null)
            {
                foreach (var ach in AchievementManager.Instance.unlockedAchievements) data.unlockedAchievements.Add(ach);
            }

            if (ModifierManager.Instance != null)
            {
                foreach (var mod in ModifierManager.Instance.GetActiveModifiers())
                {
                    data.activeModifiers.Add(new ModifierSaveData { data = mod.data, remainingMonths = mod.remainingMonths });
                }
            }

            if (GlobalMapManager.Instance != null)
            {
                data.alliances = new List<SuperpowerType>(GlobalMapManager.Instance.alliedSuperpowers);
            }

            if (CrimeManager.Instance != null)
            {
                data.crimeRate = CrimeManager.Instance.globalCrimeRate;
            }

            if (EducationManager.Instance != null)
            {
                data.educationLevel = EducationManager.Instance.globalEducationLevel;
            }

            if (HealthManager.Instance != null)
            {
                data.healthLevel = HealthManager.Instance.globalHealthLevel;
            }

            if (MilitaryManager.Instance != null)
            {
                data.armyStrength = MilitaryManager.Instance.armyStrength;
                data.trainingLevel = MilitaryManager.Instance.trainingLevel;
                data.armyLoyalty = MilitaryManager.Instance.armyLoyalty;
            }

            if (CultureManager.Instance != null)
            {
                data.cultureLevel = CultureManager.Instance.cultureLevel;
            }

            if (CorruptionManager.Instance != null)
            {
                data.corruptionLevel = CorruptionManager.Instance.corruptionLevel;
                data.blackMarketMoney = CorruptionManager.Instance.blackMarketMoney;
            }

            if (DynastyManager.Instance != null)
            {
                data.rulerData = BuildHeirSaveData(DynastyManager.Instance.currentRuler);
                foreach (var heir in DynastyManager.Instance.heirs)
                {
                    data.heirs.Add(BuildHeirSaveData(heir));
                }
            }

            if (StatsManager.Instance != null)
            {
                foreach (var b in StatsManager.Instance.GetTrackedBuildings())
                {
                    BuildingSaveData bs = new BuildingSaveData {
                        buildingId = b.buildingId,
                        posX = b.GridPosition.x,
                        posZ = b.GridPosition.z,
                        health = b.currentHealth
                    };
                    foreach (var resId in b.inventory.GetStoredResourceIds())
                    {
                        bs.inventory.Add(new InventorySaveData { resourceId = resId, amount = b.inventory.GetAmountById(resId) });
                    }
                    data.buildings.Add(bs);
                }
            }

            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);

            Debug.Log($"Game saved to {path}");

            if (SteamManager.Instance != null)
                SteamManager.Instance.TriggerCloudSave(fileName);
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

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.currentMode = data.mode;
                GameStateManager.Instance.sandboxSettings = data.sandboxSettings;
                // Loading mission asset by ID would happen here via Resources.Load
            }

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

            if (FactionManager.Instance != null)
            {
                FactionManager.Instance.LoadRelationSaveData(data.relations);
            }

            if (UI.TutorialManager.Instance != null)
            {
                UI.TutorialManager.Instance.completedSteps.Clear();
                foreach (var step in data.completedTutorialSteps) UI.TutorialManager.Instance.completedSteps.Add(step);
            }

            if (AchievementManager.Instance != null)
            {
                AchievementManager.Instance.unlockedAchievements.Clear();
                foreach (var ach in data.unlockedAchievements) AchievementManager.Instance.unlockedAchievements.Add(ach);
            }

            if (ModifierManager.Instance != null)
            {
                List<ModifierManager.ActiveModifier> mods = new List<ModifierManager.ActiveModifier>();
                foreach (var msd in data.activeModifiers)
                {
                    mods.Add(new ModifierManager.ActiveModifier { data = msd.data, remainingMonths = msd.remainingMonths });
                }
                ModifierManager.Instance.LoadModifiers(mods);
            }

            if (GlobalMapManager.Instance != null)
            {
                GlobalMapManager.Instance.alliedSuperpowers = new List<SuperpowerType>(data.alliances);
            }

            if (CrimeManager.Instance != null)
            {
                CrimeManager.Instance.globalCrimeRate = data.crimeRate;
            }

            if (EducationManager.Instance != null)
            {
                EducationManager.Instance.globalEducationLevel = data.educationLevel;
            }

            if (HealthManager.Instance != null)
            {
                HealthManager.Instance.globalHealthLevel = data.healthLevel;
            }

            if (MilitaryManager.Instance != null)
            {
                MilitaryManager.Instance.armyStrength = data.armyStrength;
                MilitaryManager.Instance.trainingLevel = data.trainingLevel;
                MilitaryManager.Instance.armyLoyalty = data.armyLoyalty;
            }

            if (CultureManager.Instance != null)
            {
                CultureManager.Instance.cultureLevel = data.cultureLevel;
            }

            if (CorruptionManager.Instance != null)
            {
                CorruptionManager.Instance.corruptionLevel = data.corruptionLevel;
                CorruptionManager.Instance.blackMarketMoney = data.blackMarketMoney;
            }

            if (DynastyManager.Instance != null)
            {
                DynastyManager.Instance.currentRuler = BuildHeirFromSaveData(data.rulerData);
                if (DynastyManager.Instance.currentRuler == null)
                {
                    DynastyManager.Instance.currentRuler = new Heir
                    {
                        heirName = "El Presidente",
                        age = 45,
                        gender = HeirGender.Male,
                        isAlive = true,
                        loyaltyToRuler = 100f
                    };
                    DynastyManager.Instance.currentRuler.GenerateRandomStats();
                }

                DynastyManager.Instance.heirs.Clear();
                if (data.heirs != null)
                {
                    foreach (var heirData in data.heirs)
                    {
                        Heir loadedHeir = BuildHeirFromSaveData(heirData);
                        if (loadedHeir != null) DynastyManager.Instance.heirs.Add(loadedHeir);
                    }
                }
                DynastyManager.Instance.UpdateHeirSupportFromFactions();
            }

            // Restore buildings
            foreach (var bs in data.buildings)
            {
                BuildingData bData = Resources.Load<BuildingData>($"Buildings/{bs.buildingId}");
                if (bData != null)
                {
                    Vector3 pos = new Vector3(bs.posX, 0, bs.posZ);
                    GameObject go = Instantiate(bData.prefab, pos, Quaternion.identity);
                    Building b = go.GetComponent<Building>();
                    b.data = bData;
                    b.GridPosition = (bs.posX, bs.posZ);
                    b.currentHealth = bs.health;
                    foreach (var invData in bs.inventory)
                    {
                        ResourceType rType = Resources.Load<ResourceType>($"Resources/{invData.resourceId}");
                        if (rType != null) b.inventory.AddResource(rType, invData.amount);
                    }
                }
            }

            Debug.Log("Game loaded successfully.");
        }

        private HeirSaveData BuildHeirSaveData(Heir heir)
        {
            if (heir == null) return null;

            HeirSaveData data = new HeirSaveData
            {
                heirName = heir.heirName,
                age = heir.age,
                gender = heir.gender,
                charisma = heir.charisma,
                cruelty = heir.cruelty,
                intelligence = heir.intelligence,
                military = heir.military,
                loyaltyToRuler = heir.loyaltyToRuler,
                jealousy = heir.jealousy,
                isAlive = heir.isAlive,
                secretTraits = new List<SecretTrait>(heir.secretTraits)
            };

            foreach (var entry in heir.factionSupport)
            {
                data.factionSupport.Add(new HeirSupportSaveData { faction = entry.Key, support = entry.Value });
            }

            return data;
        }

        private Heir BuildHeirFromSaveData(HeirSaveData data)
        {
            if (data == null) return null;

            Heir heir = new Heir
            {
                heirName = data.heirName,
                age = data.age,
                gender = data.gender,
                charisma = data.charisma,
                cruelty = data.cruelty,
                intelligence = data.intelligence,
                military = data.military,
                loyaltyToRuler = data.loyaltyToRuler,
                jealousy = data.jealousy,
                isAlive = data.isAlive,
                secretTraits = new List<SecretTrait>(data.secretTraits)
            };

            foreach (var support in data.factionSupport)
            {
                heir.factionSupport[support.faction] = support.support;
            }

            return heir;
        }

        public List<SaveMetadata> GetAllSaveFiles()
        {
            List<SaveMetadata> results = new List<SaveMetadata>();
            string path = Application.persistentDataPath;
            if (!Directory.Exists(path)) return results;

            string[] files = Directory.GetFiles(path, "*.json");

            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                results.Add(new SaveMetadata
                {
                    fileName = info.Name,
                    islandName = "Island Paradise",
                    date = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                });
            }

            return results;
        }

        public void DeleteSave(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Deleted save: {fileName}");
            }
        }
    }
}
