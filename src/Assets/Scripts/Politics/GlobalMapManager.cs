using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class GlobalMapManager : MonoBehaviour
    {
        public static GlobalMapManager Instance { get; private set; }

        public List<Superpower> superpowers = new List<Superpower>();
        public List<Agent> agentPool = new List<Agent>();
        public List<Bodyguard> bodyguards = new List<Bodyguard>();
        public List<ActiveMission> activeMissions = new List<ActiveMission>();
        public List<Colonization.Colony> discoveredIslands = new List<Colonization.Colony>();

        [Header("Diplomacy")]
        public List<Data.Sanction> activeSanctions = new List<Data.Sanction>();
        public List<SuperpowerType> alliedSuperpowers = new List<SuperpowerType>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            InitializeBodyguards();
        }

        private void Update()
        {
            UpdateMissions(Time.deltaTime);
        }

        private void UpdateMissions(float deltaTime)
        {
            for (int i = activeMissions.Count - 1; i >= 0; i--)
            {
                activeMissions[i].Update(deltaTime);
                if (activeMissions[i].IsComplete)
                {
                    CompleteMission(activeMissions[i]);
                    activeMissions.RemoveAt(i);
                }
            }
        }

        public void StartAlliance(SuperpowerType superpower)
        {
            if (!alliedSuperpowers.Contains(superpower))
            {
                alliedSuperpowers.Add(superpower);
                Debug.Log($"Alliance formed with {superpower}!");
            }
        }

        public void BreakAlliance(SuperpowerType superpower)
        {
            if (alliedSuperpowers.Contains(superpower))
            {
                alliedSuperpowers.Remove(superpower);
                Debug.Log($"Alliance broken with {superpower}!");
            }
        }

        public void ApplySanction(Data.Sanction sanction)
        {
            activeSanctions.Add(sanction);
            foreach (var effect in sanction.effects)
            {
                if (Core.ModifierManager.Instance != null)
                    Core.ModifierManager.Instance.AddModifier(effect);
            }
            Debug.Log($"Sanction applied by {sanction.issuerName}: {sanction.reason}");
        }

        public void StartMission(GlobalMission template, Agent agent)
        {
            if (agent.isOnMission) return;

            ActiveMission mission = new ActiveMission
            {
                template = template,
                assignedAgent = agent,
                timeRemaining = template.duration
            };

            agent.isOnMission = true;
            activeMissions.Add(mission);
        }

        private void CompleteMission(ActiveMission mission)
        {
            mission.assignedAgent.isOnMission = false;

            // Handle Bodyguard Personal Mission Progress
            foreach (var bg in bodyguards)
            {
                if (bg == mission.assignedAgent)
                {
                    var pm = bg.GetNextMission();
                    if (pm != null && pm.missionTemplate == mission.template)
                    {
                        bg.currentMissionIndex++;
                        // Use casting to access agent skills if needed, or make skills protected/internal
                        bg.stealth += pm.skillReward;
                        Debug.Log($"{bg.agentName} completed personal mission: {pm.title}");
                    }
                }
            }

            float successChance = mission.assignedAgent.GetSuccessChance(mission.template);
            bool success = UnityEngine.Random.Range(0, 100) < successChance;

            if (success)
            {
                foreach (var sp in superpowers)
                {
                    if (sp.missionTemplates.Contains(mission.template))
                    {
                        sp.relations += mission.template.rewardRelations;
                        break;
                    }
                }
                Debug.Log($"Mission Successful: {mission.template.title}");
            }
            else
            {
                Debug.Log($"Mission Failed: {mission.template.title}");
            }
        }

        public float RequestMilitaryAid(SuperpowerType againstInvader)
        {
            if (MilitaryManager.Instance == null || alliedSuperpowers.Count == 0) return 0f;

            float support = 0f;
            foreach (var ally in alliedSuperpowers)
            {
                if (ally != againstInvader)
                {
                    support += 20f;
                }
            }

            MilitaryManager.Instance.AddForeignSupport(support);
            return support;
        }

        public void DiscoverIsland(string islandId, string name)
        {
            if (!discoveredIslands.Exists(i => i.islandId == islandId))
            {
                discoveredIslands.Add(new Colonization.Colony { islandId = islandId, name = name });
                Debug.Log($"New island discovered: {name}!");
            }
        }

        private void InitializeBodyguards()
        {
            AddBodyguard("El Charro Negro", 30, 80, 50, 10);
            AddBodyguard("La Mama Grande", 20, 90, 60, 10);
            AddBodyguard("El Fantasma", 95, 40, 20, 70);
            AddBodyguard("Capitan Muerto", 40, 85, 30, 50);
            AddBodyguard("La Cobre", 85, 70, 40, 20);
            AddBodyguard("El Padre", 10, 30, 90, 20);
            AddBodyguard("Gringo", 50, 70, 60, 60);
            AddBodyguard("La China", 60, 20, 40, 95);
            AddBodyguard("El Tiburon", 40, 80, 50, 30);
            AddBodyguard("El Sombra", 70, 60, 60, 40);
        }

        private void AddBodyguard(string name, float s, float c, float ch, float t)
        {
            Bodyguard bg = new Bodyguard { agentName = name, isUnique = true, stealth = s, combat = c, charisma = ch, tech = t };
            bodyguards.Add(bg);
            agentPool.Add(bg);
        }

        public List<BodyguardSaveData> GetBodyguardData()
        {
            List<BodyguardSaveData> list = new List<BodyguardSaveData>();
            foreach (var bg in bodyguards)
            {
                list.Add(new BodyguardSaveData {
                    name = bg.agentName,
                    stealth = bg.stealth,
                    combat = bg.combat,
                    missionIndex = bg.currentMissionIndex
                });
            }
            return list;
        }

        public bool OfferDynasticMarriage(SuperpowerType superpowerType)
        {
            Superpower superpower = superpowers.Find(s => s.type == superpowerType);
            if (superpower == null || DynastyManager.Instance == null) return false;

            superpower.relations += 12f;

            Heir diplomaticHeir = new Heir
            {
                heirName = $"Diplomatic Heir ({superpowerType})",
                age = UnityEngine.Random.Range(18, 30),
                gender = (HeirGender)UnityEngine.Random.Range(0, 3),
                isAlive = true,
                loyaltyToRuler = 65f
            };
            diplomaticHeir.GenerateRandomStats();
            diplomaticHeir.intelligence = Mathf.Clamp(diplomaticHeir.intelligence + 10f, 0f, 100f);
            DynastyManager.Instance.AddHeir(diplomaticHeir);

            Debug.Log($"Dynastic marriage signed with {superpowerType}. Relations improved.");
            return true;
        }

        public bool StartSmugglingMission(Agent agent, string resourceId, float amount)
        {
            if (agent == null || agent.isOnMission) return false;
            if (agent.stealth < 60f)
            {
                Debug.Log("Smuggling mission failed to start: stealth is too low.");
                return false;
            }

            float payout = Mathf.Max(0f, amount) * 14f;
            if (CorruptionManager.Instance != null)
            {
                CorruptionManager.Instance.AddBlackMarketMoney(payout);
                CorruptionManager.Instance.corruptionLevel = Mathf.Clamp(CorruptionManager.Instance.corruptionLevel + 1.5f, 0f, 100f);
            }

            agent.isOnMission = true;
            Debug.Log($"Smuggling mission complete: {resourceId} x{amount} => black market ${payout}");
            agent.isOnMission = false;
            return true;
        }

        public void LoadBodyguardData(List<BodyguardSaveData> data)
        {
            foreach (var d in data)
            {
                var bg = bodyguards.Find(b => b.agentName == d.name);
                if (bg != null)
                {
                    bg.stealth = d.stealth;
                    bg.combat = d.combat;
                    bg.currentMissionIndex = d.missionIndex;
                }
            }
        }
    }

    [System.Serializable]
    public class BodyguardSaveData
    {
        public string name;
        public float stealth;
        public float combat;
        public int missionIndex;
    }
}
