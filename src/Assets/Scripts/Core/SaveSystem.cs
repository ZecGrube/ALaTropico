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
        public class CorporationSaveData
        {
            public string name;
            public Economy.CorporationType type;
            public Economy.IndustryType industry;
            public float treasury;
            public int shares;
            public float sharePrice;
            public float stateOwn;
            public float privateOwn;
            public float foreignOwn;
        }

        [System.Serializable]
        public class CitizenSaveData
        {
            public int id;
            public string firstName;
            public string lastName;
            public AI.Gender gender;
            public int age;
            public float wealth;
            public float satisfaction;
            public float health;
            public AI.SocialClass socialClass;
            public AI.EducationLevel education;
            public AI.PersonalityTraits traits;
            public List<AI.FactionLeaningEntry> leanings;
        }

        [System.Serializable]
        public class BuildingSaveData
        {
            public string buildingId;
            public int posX;
            public int posZ;
            public float health;
            public List<InventorySaveData> inventory = new List<InventorySaveData>();
            public string ownerCorporationName;
            public float garbage;
            public bool requiresPower;
            public bool requiresWater;
        }

        [System.Serializable]
        public class ConnectionSaveData
        {
            public int nodeA_x, nodeA_z;
            public int nodeB_x, nodeB_z;
            public Systems.ConnectionType type;
            public bool isBroken;
        }

        [System.Serializable]
        public class ModifierSaveData
        {
            public Data.ModifierData data;
            public int remainingMonths;
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
        public class DistrictSaveData
        {
            public string name;
            public Color color;
            public RectInt area;
            public List<string> policyIds;
        }

        [System.Serializable]
        public class GlobalEventSaveData
        {
            public string templateId;
            public string countryName;
            public float remainingDuration;
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
            public List<CitizenSaveData> population = new List<CitizenSaveData>();
            public List<string> completedTutorialSteps = new List<string>();
            public List<string> unlockedAchievements = new List<string>();
            public List<ModifierSaveData> activeModifiers = new List<ModifierSaveData>();
            public List<Politics.SuperpowerType> alliances = new List<Politics.SuperpowerType>();
            public List<CorporationSaveData> corporations = new List<CorporationSaveData>();
            public float crimeRate;
            public float educationLevel;
            public float healthLevel;
            public float cultureLevel;
            public float armyTraining;
            public float armyLoyalty;
            public float corruptionRate;
            public float blackMarketMoney;
            public Heir currentRuler;
            public List<Heir> heirs = new List<Heir>();
            public float islandGarbage;
            public float religiousInfluence;
            public Politics.ReligiousLeader religiousLeader;
            public float cultLevel;
            public float currentResearchPoints;
            public string currentResearchId;
            public float researchProgress;
            public List<ResourceSaveData> abstractStockpiles = new List<ResourceSaveData>();
            public List<ConnectionSaveData> utilityConnections = new List<ConnectionSaveData>();
            public List<DistrictSaveData> districts = new List<DistrictSaveData>();
            public List<string> constructedLandmarkIds = new List<string>();
            public List<GlobalEventSaveData> globalEvents = new List<GlobalEventSaveData>();
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

            if (CultureManager.Instance != null)
            {
                data.cultureLevel = CultureManager.Instance.globalCultureLevel;
            }

            if (MilitaryManager.Instance != null)
            {
                data.armyTraining = MilitaryManager.Instance.trainingLevel;
                data.armyLoyalty = MilitaryManager.Instance.armyLoyalty;
            }

            if (CorruptionManager.Instance != null)
            {
                data.corruptionRate = CorruptionManager.Instance.globalCorruptionRate;
                data.blackMarketMoney = CorruptionManager.Instance.blackMarketMoney;
            }

            if (DynastyManager.Instance != null)
            {
                data.currentRuler = DynastyManager.Instance.currentRuler;
                data.heirs = new List<Heir>(DynastyManager.Instance.activeHeirs);
            }

            if (ReligionManager.Instance != null)
            {
                data.religiousInfluence = ReligionManager.Instance.religiousInfluence;
                data.religiousLeader = ReligionManager.Instance.currentLeader;
            }

            if (PersonalityCultManager.Instance != null)
            {
                data.cultLevel = PersonalityCultManager.Instance.cultLevel;
            }

            if (TechnologyManager.Instance != null)
            {
                data.currentResearchPoints = TechnologyManager.Instance.currentResearchPoints;
                data.researchProgress = TechnologyManager.Instance.researchProgress;
                if (TechnologyManager.Instance.currentResearch != null)
                    data.currentResearchId = TechnologyManager.Instance.currentResearch.techId;
            }

            if (StatsManager.Instance != null)
            {
                foreach (var kvp in StatsManager.Instance.GetAbstractStockpiles())
                {
                    data.abstractStockpiles.Add(new ResourceSaveData { resourceId = kvp.Key, amount = kvp.Value });
                }
            }

            if (AI.PopulationManager.Instance != null)
            {
                foreach (var c in AI.PopulationManager.Instance.allCitizens)
                {
                    data.population.Add(new CitizenSaveData {
                        id = c.id,
                        firstName = c.firstName,
                        lastName = c.lastName,
                        gender = c.gender,
                        age = c.age,
                        wealth = c.personalWealth,
                        satisfaction = c.satisfaction,
                        health = c.health,
                        socialClass = c.socialClass,
                        education = c.education,
                        traits = c.traits,
                        leanings = new List<AI.FactionLeaningEntry>(c.politicalLeanings)
                    });
                }
            }

            if (Economy.CorporationManager.Instance != null)
            {
                foreach (var corp in Economy.CorporationManager.Instance.corporations)
                {
                    data.corporations.Add(new CorporationSaveData {
                        name = corp.corporationName,
                        type = corp.type,
                        industry = corp.industry,
                        treasury = corp.treasury,
                        shares = corp.totalShares,
                        sharePrice = corp.sharePrice,
                        stateOwn = corp.stateOwnershipPercent,
                        privateOwn = corp.privateOwnershipPercent,
                        foreignOwn = corp.foreignOwnershipPercent
                    });
                }
            }

            if (Economy.WasteManager.Instance != null)
            {
                data.islandGarbage = Economy.WasteManager.Instance.totalGarbageOnIsland;
            }

            if (World.DistrictManager.Instance != null)
            {
                foreach (var d in World.DistrictManager.Instance.activeDistricts)
                {
                    var ds = new DistrictSaveData { name = d.districtName, color = d.districtColor, area = d.area, policyIds = new List<string>() };
                    foreach (var p in d.activePolicies) ds.policyIds.Add(p.policyId);
                    data.districts.Add(ds);
                }
            }

            if (World.LandmarkManager.Instance != null)
            {
                foreach (var l in World.LandmarkManager.Instance.constructedLandmarks) data.constructedLandmarkIds.Add(l.landmarkUniqueId);
            }

            if (GlobalEventGenerator.Instance != null)
            {
                foreach (var ev in GlobalEventGenerator.Instance.activeEvents)
                {
                    data.globalEvents.Add(new GlobalEventSaveData { templateId = ev.templateId, countryName = ev.targetCountryName, remainingDuration = ev.remainingDuration });
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
                        health = b.currentHealth,
                        ownerCorporationName = b.ownerCorporation != null ? b.ownerCorporation.corporationName : "",
                        garbage = b.garbageAccumulated
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

            if (CultureManager.Instance != null)
            {
                CultureManager.Instance.globalCultureLevel = data.cultureLevel;
            }

            if (MilitaryManager.Instance != null)
            {
                MilitaryManager.Instance.trainingLevel = data.armyTraining;
                MilitaryManager.Instance.armyLoyalty = data.armyLoyalty;
                MilitaryManager.Instance.UpdateMilitaryStrength();
            }

            if (CorruptionManager.Instance != null)
            {
                CorruptionManager.Instance.globalCorruptionRate = data.corruptionRate;
                CorruptionManager.Instance.blackMarketMoney = data.blackMarketMoney;
            }

            if (DynastyManager.Instance != null)
            {
                DynastyManager.Instance.currentRuler = data.currentRuler;
                DynastyManager.Instance.activeHeirs = new List<Heir>(data.heirs);
            }

            if (ReligionManager.Instance != null)
            {
                ReligionManager.Instance.religiousInfluence = data.religiousInfluence;
                ReligionManager.Instance.currentLeader = data.religiousLeader;
            }

            if (PersonalityCultManager.Instance != null)
            {
                PersonalityCultManager.Instance.cultLevel = data.cultLevel;
            }

            if (Economy.WasteManager.Instance != null)
            {
                Economy.WasteManager.Instance.totalGarbageOnIsland = data.islandGarbage;
            }

            if (TechnologyManager.Instance != null)
            {
                TechnologyManager.Instance.currentResearchPoints = data.currentResearchPoints;
                TechnologyManager.Instance.researchProgress = data.researchProgress;
                if (!string.IsNullOrEmpty(data.currentResearchId))
                {
                    var tech = TechnologyManager.Instance.allTechnologies.Find(t => t.techId == data.currentResearchId);
                    if (tech != null) TechnologyManager.Instance.currentResearch = tech;
                }
            }

            if (StatsManager.Instance != null)
            {
                Dictionary<string, float> abstractS = new Dictionary<string, float>();
                foreach (var rs in data.abstractStockpiles) abstractS.Add(rs.resourceId, rs.amount);
                StatsManager.Instance.SetAbstractStockpiles(abstractS);
            }

            if (Economy.CorporationManager.Instance != null)
            {
                Economy.CorporationManager.Instance.corporations.Clear();
                foreach (var cs in data.corporations)
                {
                    var corp = new Economy.Corporation(cs.name, cs.type, cs.industry) {
                        treasury = cs.treasury,
                        totalShares = cs.shares,
                        sharePrice = cs.sharePrice,
                        stateOwnershipPercent = cs.stateOwn,
                        privateOwnershipPercent = cs.privateOwn,
                        foreignOwnershipPercent = cs.foreignOwn
                    };
                    Economy.CorporationManager.Instance.corporations.Add(corp);
                }
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
                    if (TileManager.Instance != null)
                        TileManager.Instance.OccupyTile(new Vector2Int(bs.posX, bs.posZ), go);
                    b.currentHealth = bs.health;
                    b.garbageAccumulated = bs.garbage;
                    if (!string.IsNullOrEmpty(bs.ownerCorporationName) && Economy.CorporationManager.Instance != null)
                    {
                        var corp = Economy.CorporationManager.Instance.corporations.Find(c => c.corporationName == bs.ownerCorporationName);
                        if (corp != null)
                        {
                            b.ownerCorporation = corp;
                            corp.ownedBuildings.Add(b);
                        }
                    }
                    foreach (var invData in bs.inventory)
                    {
                        ResourceType rType = Resources.Load<ResourceType>($"Resources/{invData.resourceId}");
                        if (rType != null) b.inventory.AddResource(rType, invData.amount);
                    }
                }
            }

            Debug.Log("Game loaded successfully.");
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
