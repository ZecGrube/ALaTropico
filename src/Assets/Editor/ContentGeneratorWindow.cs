using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.Politics;
using System.Linq;

namespace CaudilloBay.Editor
{
    /// <summary>
    /// Основное окно генератора контента для игры Caudillo Bay.
    /// Позволяет массово создавать ассеты технологий, событий, зданий и др.
    /// </summary>
    public class ContentGeneratorWindow : EditorWindow
    {
        [MenuItem("Tools/Content Generator")]
        public static void ShowWindow()
        {
            GetWindow<ContentGeneratorWindow>("Content Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Caudillo Bay Content Generator", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Generate All Content", GUILayout.Height(40))) GenerateAll();

            GUILayout.Space(10);
            if (GUILayout.Button("Generate Technologies (30+)")) GenerateTechnologies();
            if (GUILayout.Button("Generate Events (20+)")) GenerateEvents();
            if (GUILayout.Button("Generate Buildings (50+)")) GenerateBuildings();
            if (GUILayout.Button("Generate Decrees (30+)")) GenerateDecrees();
            if (GUILayout.Button("Generate Achievements (30+)")) GenerateAchievements();
            if (GUILayout.Button("Generate Campaign Missions (5-10)")) GenerateCampaignMissions();

            GUILayout.Space(20);
            if (GUILayout.Button("Update Localization Files")) UpdateLocalization();
            if (GUILayout.Button("Clear Generated Content (Dangerous!)")) ClearGeneratedContent();
        }

        public static void GenerateAll()
        {
            GenerateTechnologies();
            GenerateEvents();
            GenerateBuildings();
            GenerateDecrees();
            GenerateAchievements();
            GenerateCampaignMissions();

            UpdateLocalization();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Generation Process Complete.");
        }

        #region Technologies
        public static void GenerateTechnologies()
        {
            string path = "Assets/Resources/Technologies";
            EnsureDirectory(path);

            var templates = new List<TechTemplate> {
                // Colonial
                new TechTemplate("Agricultural Science", "Colonial", 50, 1),
                new TechTemplate("Basic Forestry", "Colonial", 60, 1),
                new TechTemplate("Coastal Fishing", "Colonial", 80, 2),
                new TechTemplate("Animal Husbandry", "Colonial", 100, 2),
                new TechTemplate("Early Masonry", "Colonial", 120, 2),
                // Industrial
                new TechTemplate("Steam Power", "Industrial", 300, 4),
                new TechTemplate("Metallurgy", "Industrial", 400, 5),
                new TechTemplate("Textile Mills", "Industrial", 350, 4),
                new TechTemplate("Railroad Engineering", "Industrial", 600, 8),
                new TechTemplate("Coal Mining", "Industrial", 500, 6),
                new TechTemplate("Standardized Shipping", "Industrial", 550, 7),
                new TechTemplate("Telegraph Lines", "Industrial", 450, 5),
                new TechTemplate("Basic Sanitation", "Industrial", 400, 4),
                // Modern
                new TechTemplate("Internal Combustion", "Modern", 1500, 12),
                new TechTemplate("Oil Drilling", "Modern", 1800, 14),
                new TechTemplate("Electricity Grid", "Modern", 2000, 16),
                new TechTemplate("Radio Communications", "Modern", 1200, 10),
                new TechTemplate("Aviation Industry", "Modern", 3000, 20),
                new TechTemplate("Plastic Manufacturing", "Modern", 2500, 18),
                new TechTemplate("Modern Tourism", "Modern", 1700, 12),
                new TechTemplate("Antibiotics", "Modern", 1400, 10),
                new TechTemplate("Television", "Modern", 2200, 15),
                new TechTemplate("Digital Computing", "Modern", 4000, 24),
                new TechTemplate("Space Flight", "Modern", 8000, 36),
                new TechTemplate("Automated Assembly", "Modern", 3500, 18),
                // Future
                new TechTemplate("Advanced Robotics", "Future", 12000, 30),
                new TechTemplate("Machine Learning", "Future", 15000, 36),
                new TechTemplate("Genetic Sequencing", "Future", 18000, 40),
                new TechTemplate("Nano-Materials", "Future", 20000, 42),
                new TechTemplate("Nuclear Fusion", "Future", 50000, 60),
                new TechTemplate("Quantum Internet", "Future", 30000, 48),
                new TechTemplate("Deep Sea Mining", "Future", 25000, 42),
                new TechTemplate("Bio-Engineering", "Future", 22000, 38),
                new TechTemplate("Fusion Propulsion", "Future", 80000, 72),
                new TechTemplate("Neural Interface", "Future", 45000, 54)
            };

            int count = 0;
            foreach (var t in templates)
            {
                string id = $"tech_{t.name.ToLower().Replace(" ", "_")}";
                if (AssetExists(path, id)) continue;

                var tech = CreateInstance<Technology>();
                tech.techId = id;
                tech.techName = t.name;
                tech.description = $"{t.name} from {t.era} era. Essential for progress.";
                tech.researchCost = t.cost;
                tech.researchTime = t.time;

                SaveAsset(tech, path, id);
                count++;
                EditorUtility.DisplayProgressBar("Generating Technologies", t.name, (float)count / templates.Count);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Generated {count} technologies.");
        }

        private struct TechTemplate {
            public string name; public string era; public int cost; public float time;
            public TechTemplate(string n, string e, int c, float t) { name = n; era = e; cost = c; time = t; }
        }
        #endregion

        #region Events
        public static void GenerateEvents()
        {
            string path = "Assets/Resources/Events";
            EnsureDirectory(path);

            var categories = new[] { "Economic", "Political", "Natural", "Social", "Technological" };
            int count = 0;

            for (int i = 1; i <= 25; i++)
            {
                string category = categories[Random.Range(0, categories.Length)];
                string id = $"evt_{category.ToLower()}_{i:D3}";
                if (AssetExists(path, id)) continue;

                var evt = CreateInstance<GameEvent>();
                evt.eventId = id;
                evt.title = $"{category} Event #{i}";
                evt.description = $"A unique {category.ToLower()} situation has developed on Caudillo Bay.";
                evt.randomWeight = Random.Range(0.01f, 0.15f);

                evt.choices = new List<EventChoice> {
                    new EventChoice {
                        choiceText = "Investigate",
                        outcomeText = "The situation is under control.",
                        legitimacyChange = 5f,
                        mandateChange = 1
                    },
                    new EventChoice {
                        choiceText = "Ignore",
                        outcomeText = "People are unhappy with the neglect.",
                        legitimacyChange = -10f,
                        mandateChange = -2
                    }
                };

                SaveAsset(evt, path, id);
                count++;
                EditorUtility.DisplayProgressBar("Generating Events", evt.title, count / 25f);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Generated {count} events.");
        }
        #endregion

        #region Buildings
        public static void GenerateBuildings()
        {
            string path = "Assets/Resources/Buildings";
            string prefabPath = "Assets/Prefabs/GeneratedBuildings";
            EnsureDirectory(path);
            EnsureDirectory(prefabPath);

            var templates = new List<BuildingTemplate> {
                // Residential
                new BuildingTemplate("Peasant Hut", BuildingCategory.Residential, 100, 0, 4, 0),
                new BuildingTemplate("Worker Barracks", BuildingCategory.Residential, 500, 2, 20, 0),
                new BuildingTemplate("Colonial House", BuildingCategory.Residential, 1500, 0, 6, 0),
                new BuildingTemplate("Concrete Apartment", BuildingCategory.Residential, 5000, 2, 40, 0),
                new BuildingTemplate("Modern Condo", BuildingCategory.Residential, 15000, 4, 30, 0),
                new BuildingTemplate("Elite Mansion", BuildingCategory.Residential, 40000, 2, 8, 0),
                new BuildingTemplate("Eco-Arcology", BuildingCategory.Residential, 200000, 10, 200, 0),
                // Agricultural
                new BuildingTemplate("Banana Farm", BuildingCategory.Agricultural, 800, 10, 0, 50),
                new BuildingTemplate("Corn Field", BuildingCategory.Agricultural, 600, 8, 0, 40),
                new BuildingTemplate("Tobacco Plantation", BuildingCategory.Agricultural, 1200, 15, 0, 60),
                new BuildingTemplate("Sugar Mill", BuildingCategory.Agricultural, 2000, 20, 0, 80),
                new BuildingTemplate("Coffee Grove", BuildingCategory.Agricultural, 2500, 12, 0, 70),
                new BuildingTemplate("Cattle Ranch", BuildingCategory.Agricultural, 3500, 10, 0, 90),
                new BuildingTemplate("Fishing Pier", BuildingCategory.Agricultural, 500, 5, 0, 30),
                new BuildingTemplate("Pig Farm", BuildingCategory.Agricultural, 1500, 8, 0, 45),
                new BuildingTemplate("Vineyard", BuildingCategory.Agricultural, 6000, 15, 0, 120),
                new BuildingTemplate("Hydroponic Farm", BuildingCategory.Agricultural, 15000, 5, 0, 200),
                // Industrial
                new BuildingTemplate("Logging Camp", BuildingCategory.Industrial, 1000, 12, 0, 40),
                new BuildingTemplate("Stone Quarry", BuildingCategory.Industrial, 1500, 20, 0, 50),
                new BuildingTemplate("Sawmill", BuildingCategory.Industrial, 2500, 15, 0, 80),
                new BuildingTemplate("Brick Factory", BuildingCategory.Industrial, 5000, 25, 0, 100),
                new BuildingTemplate("Rum Distillery", BuildingCategory.Industrial, 10000, 30, 0, 150),
                new BuildingTemplate("Cigar Factory", BuildingCategory.Industrial, 15000, 40, 0, 200),
                new BuildingTemplate("Steel Mill", BuildingCategory.Industrial, 35000, 60, 0, 400),
                new BuildingTemplate("Oil Well", BuildingCategory.Industrial, 45000, 20, 0, 600),
                new BuildingTemplate("Refinery", BuildingCategory.Industrial, 80000, 50, 0, 1000),
                new BuildingTemplate("Canning Factory", BuildingCategory.Industrial, 12000, 30, 0, 180),
                new BuildingTemplate("Furniture Shop", BuildingCategory.Industrial, 8000, 15, 0, 120),
                new BuildingTemplate("Electronics Plant", BuildingCategory.Industrial, 60000, 80, 0, 800),
                new BuildingTemplate("Vehicle Factory", BuildingCategory.Industrial, 120000, 150, 0, 1500),
                new BuildingTemplate("Chemical Plant", BuildingCategory.Industrial, 40000, 40, 0, 500),
                new BuildingTemplate("Jewelry Workshop", BuildingCategory.Industrial, 25000, 10, 0, 300),
                // Infrastructure
                new BuildingTemplate("Dirt Road", BuildingCategory.Infrastructure, 20, 0, 0, 0),
                new BuildingTemplate("Asphalt Road", BuildingCategory.Infrastructure, 100, 0, 0, 0),
                new BuildingTemplate("Water Tower", BuildingCategory.Infrastructure, 2000, 2, 0, 0),
                new BuildingTemplate("Substation", BuildingCategory.Infrastructure, 3000, 4, 0, 0),
                new BuildingTemplate("Coal Power Plant", BuildingCategory.Infrastructure, 20000, 30, 0, 0),
                new BuildingTemplate("Solar Farm", BuildingCategory.Infrastructure, 50000, 10, 0, 0),
                new BuildingTemplate("Nuclear Reactor", BuildingCategory.Infrastructure, 250000, 50, 0, 0),
                new BuildingTemplate("Warehouse", BuildingCategory.Infrastructure, 5000, 5, 500, 0),
                new BuildingTemplate("Cargo Port", BuildingCategory.Infrastructure, 80000, 100, 2000, 0),
                new BuildingTemplate("Airport Terminal", BuildingCategory.Infrastructure, 150000, 200, 1000, 0),
                new BuildingTemplate("Police Station", BuildingCategory.Infrastructure, 10000, 15, 0, 0),
                new BuildingTemplate("Fire Station", BuildingCategory.Infrastructure, 8000, 10, 0, 0),
                // Tourism
                new BuildingTemplate("Cheap Hotel", BuildingCategory.Tourism, 8000, 10, 0, 0),
                new BuildingTemplate("Beach Club", BuildingCategory.Tourism, 12000, 15, 0, 0),
                new BuildingTemplate("Luxury Resort", BuildingCategory.Tourism, 100000, 80, 0, 0),
                new BuildingTemplate("Casino Royale", BuildingCategory.Tourism, 250000, 120, 0, 0),
                new BuildingTemplate("Souvenir Market", BuildingCategory.Tourism, 3000, 5, 0, 0),
                new BuildingTemplate("National Museum", BuildingCategory.Tourism, 40000, 20, 0, 0),
                new BuildingTemplate("Botanical Garden", BuildingCategory.Tourism, 25000, 12, 0, 0),
                // Government/Unique
                new BuildingTemplate("Town Hall", BuildingCategory.Government, 20000, 20, 0, 0),
                new BuildingTemplate("Ministry Office", BuildingCategory.Government, 50000, 100, 0, 0),
                new BuildingTemplate("Presidential Palace", BuildingCategory.Government, 500000, 250, 0, 0),
                new BuildingTemplate("High Court", BuildingCategory.Government, 80000, 40, 0, 0),
                new BuildingTemplate("Central Bank", BuildingCategory.Government, 120000, 60, 0, 0)
            };

            int count = 0;
            foreach (var t in templates)
            {
                string id = $"building_{t.name.ToLower().Replace(" ", "_")}";
                if (AssetExists(path, id)) continue;

                var data = CreateInstance<BuildingData>();
                data.buildingId = id;
                data.buildingName = t.name;
                data.category = t.category;
                data.workersRequired = t.workers;

                // Formula: cost = baseCost * (workersRequired^0.5) * (production^0.7)
                float workerPart = t.workers > 0 ? Mathf.Pow(t.workers, 0.5f) : 1f;
                float prodPart = t.productionValue > 0 ? Mathf.Pow(t.productionValue, 0.7f) : 1f;
                float calculatedCost = t.baseCost * workerPart * prodPart;

                data.buildCosts = new List<ResourceCost> { new ResourceCost { amount = Mathf.RoundToInt(calculatedCost) } };
                data.buildTime = 1f + (calculatedCost / 5000f);
                data.maintenanceCost = calculatedCost * 0.02f;
                data.pollutionOutput = t.category == BuildingCategory.Industrial ? t.workers * 0.5f : t.workers * 0.05f;
                data.storageCapacity = t.capacity;

                // Create Prefab
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = data.buildingName;
                // Add building component if it exists, or just leave as is.
                string pfbFile = $"{prefabPath}/{id}.prefab";
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(cube, pfbFile);
                data.prefab = prefab;
                DestroyImmediate(cube);

                SaveAsset(data, path, id);
                count++;
                EditorUtility.DisplayProgressBar("Generating Buildings", data.buildingName, (float)count / templates.Count);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Generated {count} buildings and prefabs.");
        }

        private struct BuildingTemplate {
            public string name; public BuildingCategory category; public int baseCost; public int workers; public int capacity; public float productionValue;
            public BuildingTemplate(string n, BuildingCategory c, int cost, int w, int cap, float prod) { name = n; category = c; baseCost = cost; workers = w; capacity = cap; productionValue = prod; }
        }
        #endregion

        #region Decrees
        public static void GenerateDecrees()
        {
            string path = "Assets/Resources/Decrees";
            EnsureDirectory(path);

            var decreeNames = new[] { "Tax Cut", "Mandatory Voting", "Martial Law", "Industrial Subsidy", "Education Grant", "Healthcare Reform", "Land Distribution", "Nationalization", "Free Press", "Censorship", "Export Tax", "Minimum Wage", "Child Labor Ban", "Amnesty", "Propaganda Drive", "Scientific Funding", "Green Initiative", "Tourism Campaign", "Trade Deal", "Public Works" };
            int count = 0;

            for (int i = 0; i < 30; i++)
            {
                string baseName = decreeNames[i % decreeNames.Length];
                string name = i < decreeNames.Length ? baseName : $"{baseName} II";
                string id = $"decree_{name.ToLower().Replace(" ", "_")}";
                if (AssetExists(path, id)) continue;

                var decree = CreateInstance<Decree>();
                decree.decreeName = name;
                decree.description = $"Official decree of Caudillo Bay regarding {name.ToLower()}.";
                decree.mandateCost = 10 + (i * 5);
                decree.taxModifier = Random.Range(-0.05f, 0.15f);
                decree.wageModifier = Random.Range(-0.05f, 0.15f);

                decree.loyaltyEffects = new List<FactionEffect> {
                    new FactionEffect { faction = FactionType.Capitalists, effect = Random.Range(-10, 10) },
                    new FactionEffect { faction = FactionType.Communists, effect = Random.Range(-10, 10) }
                };

                SaveAsset(decree, path, id);
                count++;
                EditorUtility.DisplayProgressBar("Generating Decrees", name, count / 30f);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Generated {count} decrees.");
        }
        #endregion

        #region Achievements
        public static void GenerateAchievements()
        {
            string path = "Assets/Resources/Achievements";
            EnsureDirectory(path);

            var achTemplates = new List<(string name, AchievementType type, float target)> {
                ("First Shack", AchievementType.BuildBuilding, 1),
                ("Town Founder", AchievementType.BuildBuilding, 10),
                ("Metropolis", AchievementType.BuildBuilding, 100),
                ("Small Town", AchievementType.ReachPopulation, 100),
                ("Growing City", AchievementType.ReachPopulation, 1000),
                ("Massive Island", AchievementType.ReachPopulation, 10000),
                ("Saving Account", AchievementType.AccumulateWealth, 1000),
                ("Wealthy Ruler", AchievementType.AccumulateWealth, 100000),
                ("El Presidente", AchievementType.AccumulateWealth, 1000000),
                ("Survivor", AchievementType.SurviveCoup, 1),
                ("Unstoppable", AchievementType.SurviveCoup, 5)
            };

            int count = 0;
            foreach (var t in achTemplates)
            {
                string id = $"ach_{t.name.ToLower().Replace(" ", "_")}";
                if (AssetExists(path, id)) continue;

                var ach = CreateInstance<Achievement>();
                ach.achievementId = id;
                ach.titleKey = t.name;
                ach.descriptionKey = $"Complete the {t.name} milestone.";
                ach.type = t.type;
                ach.targetValue = t.target;

                SaveAsset(ach, path, id);
                count++;
                EditorUtility.DisplayProgressBar("Generating Achievements", t.name, (float)count / achTemplates.Count);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Generated {count} achievements.");
        }
        #endregion

        #region Campaign Missions
        public static void GenerateCampaignMissions()
        {
            string path = "Assets/Resources/CampaignMissions";
            EnsureDirectory(path);

            var missions = new List<(string name, string desc, float treasury)> {
                ("Colonial Start", "Found your colony and survive the first year.", 1000f),
                ("Agricultural Boom", "Focus on exporting bananas and sugar.", 2500f),
                ("Steel Heart", "Industrialize the island with heavy industry.", 5000f),
                ("Tourist Trap", "Build a luxury paradise for foreigners.", 10000f),
                ("Modern Era", "Navigate modern political complexities.", 15000f),
                ("Final Frontier", "Launch a satellite to claim space.", 50000f),
                ("The Dictator", "Maintain power against all odds.", 5000f)
            };

            int count = 0;
            foreach (var m in missions)
            {
                string id = $"mission_{m.name.ToLower().Replace(" ", "_")}";
                if (AssetExists(path, id)) continue;

                var mission = CreateInstance<CampaignMission>();
                mission.missionId = id;
                mission.missionName = m.name;
                mission.description = m.desc;
                mission.initialTreasury = m.treasury;
                mission.briefingText = $"Welcome, Caudillo. {m.desc}";

                SaveAsset(mission, path, id);
                count++;
                EditorUtility.DisplayProgressBar("Generating Missions", m.name, (float)count / missions.Count);
            }
            EditorUtility.ClearProgressBar();
            Debug.Log($"Generated {count} missions.");
        }
        #endregion

        #region Factions
        public static void GenerateFactions()
        {
            Debug.Log("Faction generation skipped - FactionData is not a ScriptableObject.");
        }
        #endregion

        #region Localization
        public static void UpdateLocalization()
        {
            string enPath = "Assets/Resources/Localization/en.json";
            if (!File.Exists(enPath))
            {
                EnsureDirectory("Assets/Resources/Localization");
                File.WriteAllText(enPath, "{ \"items\": [] }");
            }

            string json = File.ReadAllText(enPath);
            LocalizationData data = JsonUtility.FromJson<LocalizationData>(json);
            if (data.items == null) data.items = new List<LocalizationItem>();

            // Add keys from generated assets
            AddKeysFromFolder<Technology>(data, "Assets/Resources/Technologies", t => t.techId + "_desc", t => t.description);
            AddKeysFromFolder<GameEvent>(data, "Assets/Resources/Events", e => e.eventId + "_desc", e => e.description);
            AddKeysFromFolder<BuildingData>(data, "Assets/Resources/Buildings", b => b.buildingId + "_desc", b => b.description);
            AddKeysFromFolder<Decree>(data, "Assets/Resources/Decrees", d => d.decreeName.ToLower().Replace(" ", "_") + "_desc", d => d.description);

            string updatedJson = JsonUtility.ToJson(data, true);
            File.WriteAllText(enPath, updatedJson);
            Debug.Log("Localization en.json updated.");
        }

        private static void AddKeysFromFolder<T>(LocalizationData data, string folder, System.Func<T, string> keyGen, System.Func<T, string> valGen) where T : UnityEngine.Object
        {
            if (!Directory.Exists(folder)) return;
            string[] files = Directory.GetFiles(folder, "*.asset");
            foreach (var file in files)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(file);
                if (asset != null)
                {
                    string key = keyGen(asset);
                    if (!data.items.Any(i => i.key == key))
                    {
                        data.items.Add(new LocalizationItem { key = key, value = valGen(asset) });
                    }
                }
            }
        }

        [System.Serializable]
        public class LocalizationData { public List<LocalizationItem> items; }
        [System.Serializable]
        public class LocalizationItem { public string key; public string value; }
        #endregion

        #region Helpers
        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static bool AssetExists(string path, string id)
        {
            return File.Exists($"{path}/{id}.asset");
        }

        private static void SaveAsset(UnityEngine.Object asset, string path, string id)
        {
            AssetDatabase.CreateAsset(asset, $"{path}/{id}.asset");
        }

        public static void ClearGeneratedContent()
        {
            if (!EditorUtility.DisplayDialog("Clear Content", "Are you sure you want to delete ALL generated content?", "Yes", "No"))
                return;

            string[] dirs = { "Technologies", "Events", "Buildings", "Decrees", "Achievements", "CampaignMissions" };
            foreach (var dir in dirs)
            {
                string path = $"Assets/Resources/{dir}";
                if (Directory.Exists(path)) Directory.Delete(path, true);
            }
            AssetDatabase.Refresh();
            Debug.Log("Generated content cleared.");
        }
        #endregion
    }
}
