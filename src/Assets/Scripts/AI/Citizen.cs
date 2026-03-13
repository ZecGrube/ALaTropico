using UnityEngine;
using UnityEngine.AI;
using CaudilloBay.World;
using CaudilloBay.Data;

namespace CaudilloBay.AI
{
    public enum CitizenState { Idle, GoingToWork, Working, GoingHome, SeekingFood }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Citizen : MonoBehaviour
    {
        [Header("Profile")]
        public int id;
        public CitizenState currentState = CitizenState.Idle;
        public float satisfaction = 50f;
        public float fearOfCrime = 0f;
        public float educationLevel = 0f;
        public float health = 100f;

        [Header("Associations")]
        public ResidentialBuilding home;
        public Building workplace;

        [Header("Needs")]
        public float hunger = 0f;
        public float hungerRate = 0.1f;

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateNeeds();
            UpdateBehavior();
        }

        private void UpdateNeeds()
        {
            hunger += hungerRate * Time.deltaTime;
            if (hunger > 80f)
            {
                // Trigger seek food logic
            }

            // Update fear and satisfaction based on crime
            if (Core.CrimeManager.Instance != null)
            {
                float localCrime = Core.CrimeManager.Instance.GetLocalCrimeRate(transform.position);
                fearOfCrime = Mathf.Lerp(fearOfCrime, localCrime, 0.01f);

                // Satisfaction penalty from fear
                if (fearOfCrime > 20f)
                {
                    satisfaction -= (fearOfCrime - 20f) * 0.001f * Time.deltaTime;
                }
            }
        }

        private void UpdateBehavior()
        {
            switch (currentState)
            {
                case CitizenState.Idle:
                    if (hunger > 70f)
                    {
                        SeekFood();
                    }
                    else if (workplace != null && IsWorkTime())
                        GoToWork();
                    break;
                case CitizenState.GoingToWork:
                    if (agent.remainingDistance < 1f)
                    {
                        currentState = CitizenState.Working;
                        agent.isStopped = true;
                    }
                    break;
                case CitizenState.Working:
                    if (!IsWorkTime())
                        GoHome();
                    break;
                case CitizenState.GoingHome:
                    if (agent.remainingDistance < 1f)
                    {
                        currentState = CitizenState.Idle;
                        agent.isStopped = true;
                    }
                    break;
                case CitizenState.SeekingFood:
                    if (agent.remainingDistance < 1f)
                    {
                        Eat();
                        GoHome();
                    }
                    break;
            }
        }

        private bool IsWorkTime()
        {
            // Placeholder for game time logic
            return true;
        }

        public void GoToWork()
        {
            if (workplace == null) return;
            currentState = CitizenState.GoingToWork;
            agent.isStopped = false;
            agent.SetDestination(workplace.transform.position);
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
            // Find nearest market or home with food
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
                satisfaction = Mathf.Min(satisfaction + 10f, 100f);
                health = Mathf.Min(health + 5f, 100f);
            }
        }

        public void UpdateHealth(float delta)
        {
            health = Mathf.Clamp(health + delta, 0, 100);
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (PopulationManager.Instance != null)
            {
                PopulationManager.Instance.Die(this);
            }
        }
    }
}
