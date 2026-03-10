using UnityEngine;
using UnityEngine.AI;

namespace CaudilloBay.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BuilderAI : MonoBehaviour
    {
        public enum BuilderState { Idle, MovingToBuild, Building }
        public BuilderState currentState = BuilderState.Idle;
        public float buildRange = 2f;

        private NavMeshAgent agent;
        private Transform targetConstruction;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            switch (currentState)
            {
                case BuilderState.Idle:
                    if (targetConstruction != null)
                    {
                        currentState = BuilderState.MovingToBuild;
                        agent.SetDestination(targetConstruction.position);
                    }
                    break;

                case BuilderState.MovingToBuild:
                    if (targetConstruction == null)
                    {
                        currentState = BuilderState.Idle;
                        agent.ResetPath();
                        break;
                    }

                    if (Vector3.Distance(transform.position, targetConstruction.position) <= buildRange)
                    {
                        currentState = BuilderState.Building;
                        agent.ResetPath();
                        Debug.Log("Builder started building...");
                    }
                    break;

                case BuilderState.Building:
                    if (targetConstruction == null)
                    {
                        currentState = BuilderState.Idle;
                        break;
                    }

                    // For now, just "building" for a moment
                    // In real logic, construction progress would increase here.
                    break;
            }
        }

        public void SetTargetConstruction(Transform target)
        {
            targetConstruction = target;
            currentState = BuilderState.MovingToBuild;
            agent.SetDestination(targetConstruction.position);
        }
    }
}
