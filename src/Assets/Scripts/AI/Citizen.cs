using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Data;
using CaudilloBay.Politics;

namespace CaudilloBay.AI
{
    public enum CitizenState { Idle, GoingToWork, Working, GoingHome, SeekingFood, Protesting }
    public enum SocialClass { Poor, WorkingClass, MiddleClass, Rich }
    public enum EducationLevel { None, Basic, Secondary, Vocational, Higher }
    public enum Gender { Male, Female }

    [System.Serializable]
    public struct PersonalityTraits
    {
        public float aggressiveness;
        public float ambition;
        public float charisma;
        public float loyalty;
        public float greed;
        public float faith;
        public float curiosity;
    }

    [System.Serializable]
    public struct FactionLeaningEntry
    {
        public FactionType faction;
        public float value;
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Citizen : MonoBehaviour
    {
        [Header("Identity")]
        public int id;
        public string firstName;
        public string lastName;
        public Gender gender;
        public int age;
        public CitizenState currentState = CitizenState.Idle;

        [Header("Socio-Economics")]
        public SocialClass socialClass = SocialClass.WorkingClass;
        public EducationLevel education = EducationLevel.Basic;
        public float personalWealth = 100f;
        public float salary = 50f;

        [Header("Social")]
        public Family family;
        public List<int> friendIds = new List<int>();

        [Header("Traits & Politics")]
        public PersonalityTraits traits;
        public List<FactionLeaningEntry> politicalLeanings = new List<FactionLeaningEntry>();

        [Header("Needs & Happiness")]
        public float satisfaction = 50f;
        public float fearOfCrime = 0f;
        public float health = 100f;
        public float hunger = 0f;
        public float hungerRate = 0.1f;

        [Header("Associations")]
        public ResidentialBuilding home;
        public Building workplace;

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            if (string.IsNullOrEmpty(firstName)) GenerateRandomIdentity();
        }

        private void GenerateRandomIdentity()
        {
            firstName = "Juan"; // Placeholder generator
            lastName = "Perez";
            gender = Random.value > 0.5f ? Gender.Male : Gender.Female;
            age = Random.Range(18, 60);

            traits = new PersonalityTraits {
                aggressiveness = Random.Range(0, 100),
                ambition = Random.Range(0, 100),
                charisma = Random.Range(0, 100),
                loyalty = Random.Range(0, 100),
                greed = Random.Range(0, 100),
                faith = Random.Range(0, 100),
                curiosity = Random.Range(0, 100)
            };

            foreach (FactionType type in System.Enum.GetValues(typeof(FactionType)))
            {
                politicalLeanings.Add(new FactionLeaningEntry { faction = type, value = 50f });
            }
        }

        private void Update()
        {
            UpdateNeeds();
            UpdateBehavior();
        }

        private void UpdateNeeds()
        {
            hunger += hungerRate * Time.deltaTime;

            // Health decay with age
            if (age > 60) health -= 0.001f * Time.deltaTime;

            if (Core.CrimeManager.Instance != null)
            {
                float localCrime = Core.CrimeManager.Instance.GetLocalCrimeRate(transform.position);
                fearOfCrime = Mathf.Lerp(fearOfCrime, localCrime, 0.01f);
            }

            CalculateHappiness();
        }

        public void CalculateHappiness()
        {
            float targetSatisfaction = 100f - hunger;
            targetSatisfaction -= fearOfCrime * 0.5f;
            targetSatisfaction += (health - 50f);
            targetSatisfaction += Mathf.Log10(personalWealth + 1) * 10f;

            // City Services factors
            if (home != null)
            {
                targetSatisfaction -= home.garbageAccumulated * 2f; // Penalty for trash
            }

            satisfaction = Mathf.Lerp(satisfaction, Mathf.Clamp(targetSatisfaction, 0, 100), 0.05f);
        }

        private void UpdateBehavior()
        {
            switch (currentState)
            {
                case CitizenState.Idle:
                    if (hunger > 70f) SeekFood();
                    else if (workplace != null && IsWorkTime()) GoToWork();
                    break;
                case CitizenState.GoingToWork:
                    if (agent.remainingDistance < 1f) { currentState = CitizenState.Working; agent.isStopped = true; }
                    break;
                case CitizenState.Working:
                    if (!IsWorkTime()) GoHome();
                    break;
                case CitizenState.GoingHome:
                    if (agent.remainingDistance < 1f) { currentState = CitizenState.Idle; agent.isStopped = true; }
                    break;
                case CitizenState.SeekingFood:
                    if (agent.remainingDistance < 1f) { Eat(); GoHome(); }
                    break;
            }
        }

        private bool IsWorkTime() => true; // Simulation time placeholder

        public void GoToWork()
        {
            if (workplace == null) return;
            currentState = CitizenState.GoingToWork;
            agent.isStopped = false;

            // Check if workplace is far and if there's a transport link
            float distance = Vector3.Distance(transform.position, workplace.transform.position);
            if (distance > 100f)
            {
                // Priority: use public transport if available
                BusStop nearestStop = FindNearestBusStop();
                if (nearestStop != null)
                {
                    Debug.Log($"Citizen {id} is taking the bus to work.");
                    agent.SetDestination(nearestStop.transform.position);
                    return;
                }
                Debug.Log($"Citizen {id} is commuting to a distant workplace (no transport).");
            }

            agent.SetDestination(workplace.transform.position);
        }

        private BusStop FindNearestBusStop()
        {
            foreach (var b in StatsManager.Instance.GetTrackedBuildings())
            {
                if (b is BusStop && Vector3.Distance(transform.position, b.transform.position) < 50f)
                {
                    return (BusStop)b;
                }
            }
            return null;
        }

        public void GoHome()
        {
            if (home == null) return;
            currentState = CitizenState.GoingHome;
            agent.isStopped = false;
            agent.SetDestination(home.transform.position);
        }

        public void SeekFood()
        {
            if (home != null && home.inventory.HasResource(home.foodType, 1.0f))
            {
                currentState = CitizenState.SeekingFood;
                agent.isStopped = false;
                agent.SetDestination(home.transform.position);
            }
        }

        private void Eat()
        {
            if (home != null && home.inventory.HasResource(home.foodType, 1.0f))
            {
                home.inventory.RemoveResource(home.foodType, 1.0f);
                hunger = 0f;
                personalWealth -= 5f; // Cost of food
            }
        }

        public void UpdateHealth(float delta)
        {
            health = Mathf.Clamp(health + delta, 0, 100);
            if (health <= 0) PopulationManager.Instance.Die(this);
        }
    }
}
