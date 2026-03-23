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

        [Header("Diplomacy")]
        public List<Data.Sanction> activeSanctions = new List<Data.Sanction>();
        public List<SuperpowerType> alliedSuperpowers = new List<SuperpowerType>();

        [Header("Global Resolutions")]
        public List<Data.ResolutionTemplate> allResolutions = new List<Data.ResolutionTemplate>();
        public List<Data.ResolutionInstance> activeResolutions = new List<Data.ResolutionInstance>();

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

        public void ProcessMonthlyGlobalState()
        {
            if (GlobalEventGenerator.Instance != null)
                GlobalEventGenerator.Instance.MonthlyTick();

            if (RegionalPoliticsManager.Instance != null)
                RegionalPoliticsManager.Instance.UpdateMonthly();

            if (AllianceManager.Instance != null)
                AllianceManager.Instance.MonthlyUpdate();

            if (SpyNetworkManager.Instance != null)
                SpyNetworkManager.Instance.UpdateMonthly();

            // Organizations dues checked monthly
            if (OrganizationManager.Instance != null)
                OrganizationManager.Instance.ProcessYearlyDues(); // Simplified: check monthly or handle counter
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

                // Shadow Rewards
                if (mission.template.title.Contains("Smuggling") && Core.CorruptionManager.Instance != null)
                {
                    Core.CorruptionManager.Instance.AddBlackMarketMoney(500f);
                }

                if (mission.template.title.Contains("Marriage") && DynastyManager.Instance != null)
                {
                    DynastyManager.Instance.AddHeir("Consort from Abroad");
                }

                if (mission.template.title.Contains("Corporate") && Economy.CorporationManager.Instance != null)
                {
                    // Trigger corporate investment or foreign branch creation
                    Economy.CorporationManager.Instance.CreateCorporation("Foreign Fruit Co", Economy.CorporationType.Foreign, Economy.IndustryType.Agriculture, new List<World.Building>());
                }
            }
            else
            {
                Debug.Log($"Mission Failed: {mission.template.title}");
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

        public void ProposeResolution(Data.ResolutionTemplate template)
        {
            if (activeResolutions.Exists(r => r.template.resolutionId == template.resolutionId)) return;

            if (Core.GlobalInfluenceManager.Instance != null && Core.GlobalInfluenceManager.Instance.globalInfluence >= template.influenceCostToPropose)
            {
                Core.GlobalInfluenceManager.Instance.globalInfluence -= template.influenceCostToPropose;
                activeResolutions.Add(new Data.ResolutionInstance { template = template, votingEndsInMonths = 3f });
                Debug.Log($"[GlobalMapManager] Resolution proposed: {template.resolutionName}");
            }
        }

        public void HostSummit()
        {
            if (Core.GlobalInfluenceManager.Instance == null) return;

            if (Core.GlobalInfluenceManager.Instance.internationalPrestige >= 70f && Economy.EconomyManager.Instance.treasuryBalance >= 10000f)
            {
                Economy.EconomyManager.Instance.AddFunds(-10000f);
                Core.GlobalInfluenceManager.Instance.AddPrestige(20f);
                Core.GlobalInfluenceManager.Instance.globalInfluence += 100f;
                Debug.Log("[GlobalMapManager] Global Summit hosted successfully! World leaders are impressed.");
            }
        }

        public void ConductGlobalVoting()
        {
            for (int i = activeResolutions.Count - 1; i >= 0; i--)
            {
                var res = activeResolutions[i];
                res.votingEndsInMonths -= 1f;
                if (res.votingEndsInMonths <= 0)
                {
                    // Calculate vote result
                    float forPercent = res.template.baseSupport;

                    // Add player influence
                    if (Core.GlobalInfluenceManager.Instance != null)
                        forPercent += (Core.GlobalInfluenceManager.Instance.globalInfluence / 10f);

                    // Alliance support
                    forPercent += alliedSuperpowers.Count * 10f;

                    if (forPercent >= res.template.requiredVotes)
                    {
                        res.isPassed = true;
                        Debug.Log($"[GlobalMapManager] Resolution passed: {res.template.resolutionName}");
                        foreach (var mod in res.template.activeModifiers)
                        {
                            if (Core.ModifierManager.Instance != null)
                                Core.ModifierManager.Instance.ApplyModifier(mod, 48); // 4 years
                        }
                    }
                    else
                    {
                        Debug.Log($"[GlobalMapManager] Resolution rejected: {res.template.resolutionName}");
                    }
                    activeResolutions.RemoveAt(i);
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
