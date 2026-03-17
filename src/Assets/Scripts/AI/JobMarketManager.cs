using UnityEngine;
using System.Collections.Generic;
using CaudilloBay.World;

namespace CaudilloBay.AI
{
    public class JobMarketManager : MonoBehaviour
    {
        public static JobMarketManager Instance { get; private set; }

        public List<JobPosition> openVacancies = new List<JobPosition>();
        public List<Union> activeUnions = new List<Union>();

        public float averageNationalSalary = 50f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterVacancy(JobPosition pos)
        {
            if (!openVacancies.Contains(pos)) openVacancies.Add(pos);
        }

        public void MonthlyUpdate()
        {
            MatchLabor();
            UpdateUnions();
        }

        private void MatchLabor()
        {
            if (PopulationManager.Instance == null) return;

            List<Citizen> unemployed = new List<Citizen>();
            foreach (var c in PopulationManager.Instance.allCitizens)
            {
                if (c.workplace == null && c.age >= 18 && c.age < 65) unemployed.Add(c);
            }

            // Sort unemployed by education and duration of unemployment
            unemployed.Sort((a, b) => b.education.CompareTo(a.education));

            // Sort vacancies by salary
            openVacancies.Sort((a, b) => b.baseSalary.CompareTo(a.baseSalary));

            for (int i = openVacancies.Count - 1; i >= 0; i--)
            {
                var vacancy = openVacancies[i];
                Citizen candidate = FindCandidate(vacancy, unemployed);

                if (candidate != null)
                {
                    Hire(candidate, vacancy);
                    unemployed.Remove(candidate);
                    openVacancies.RemoveAt(i);
                }
            }
        }

        private Citizen FindCandidate(JobPosition vacancy, List<Citizen> pool)
        {
            Citizen best = null;
            float bestScore = -1f;

            foreach (var c in pool)
            {
                if (c.education >= vacancy.requiredEducation)
                {
                    // Check salary expectation
                    if (vacancy.baseSalary < c.salaryExpectation * 0.8f) continue;

                    // Score candidate
                    float distance = Vector3.Distance(c.home != null ? c.home.transform.position : c.transform.position, vacancy.workplace.transform.position);
                    float distanceScore = Mathf.Clamp01(1f - (distance / 1000f));
                    float eduScore = (int)c.education * 0.1f;
                    float unemploymentScore = Mathf.Min(c.unemploymentDuration, 10f) * 0.1f;

                    float score = distanceScore + eduScore + unemploymentScore;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        best = c;
                    }
                }
            }
            return best;
        }

        private void Hire(Citizen c, JobPosition pos)
        {
            c.workplace = pos.workplace;
            c.salary = pos.baseSalary;
            pos.currentEmployee = c;

            if (pos.workplace is ProducerBuilding pb)
            {
                pb.employees.Add(c);
            }

            Debug.Log($"Hired {c.firstName} at {pos.workplace.data.buildingName} as {pos.title}");
        }

        private void UpdateUnions()
        {
            foreach (var union in activeUnions)
            {
                union.CalculateInfluence(PopulationManager.Instance.allCitizens.Count);

                // Strike chance based on average satisfaction and influence
                float avgSat = 0f;
                foreach(var m in union.members) avgSat += m.satisfaction;
                avgSat /= union.members.Count;

                if (avgSat < 30f && union.influence > 0.1f)
                {
                    if (Random.Range(0, 100) < (30f - avgSat) * union.influence)
                    {
                        union.StartStrike();
                    }
                }
            }
        }

        public void CreateUnion(string name)
        {
            GameObject go = new GameObject($"Union_{name}");
            Union u = go.AddComponent<Union>();
            u.unionName = name;
            activeUnions.Add(u);
        }
    }
}
