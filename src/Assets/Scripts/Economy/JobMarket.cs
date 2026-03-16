using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.AI;
using CaudilloBay.World;

namespace CaudilloBay.Economy
{
    public class JobMarket : MonoBehaviour
    {
        public static JobMarket Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ProcessJobAssignments()
        {
            List<Citizen> unemployed = PopulationManager.Instance.allCitizens.FindAll(c => c.workplace == null);
            List<ProducerBuilding> factories = new List<ProducerBuilding>();

            foreach (var b in StatsManager.Instance.GetTrackedBuildings())
            {
                if (b is ProducerBuilding pb) factories.Add(pb);
            }

            foreach (var citizen in unemployed)
            {
                foreach (var factory in factories)
                {
                    if (factory.employees.Count < 10) // Mock max employees
                    {
                        AssignJob(citizen, factory);
                        break;
                    }
                }
            }
        }

        public void AssignJob(Citizen citizen, ProducerBuilding workplace)
        {
            citizen.workplace = workplace;
            workplace.employees.Add(citizen);
            citizen.salary = workplace.data.maintenanceCost / 10f; // Mock salary
            Debug.Log($"Citizen {citizen.id} hired by {workplace.displayName}");
        }
    }
}
