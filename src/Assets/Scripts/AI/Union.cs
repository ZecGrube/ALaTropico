using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.AI
{
    [System.Serializable]
    public class Demand
    {
        public string title;
        public float value; // e.g., target salary
        public bool isSatisfied = false;
    }

    public class Union : MonoBehaviour
    {
        public string unionName;
        public List<Citizen> members = new List<Citizen>();
        public float influence = 0f;
        public List<Demand> activeDemands = new List<Demand>();

        public bool isOnStrike = false;

        public void CalculateInfluence(int totalWorkforce)
        {
            if (totalWorkforce == 0) influence = 0;
            else influence = (float)members.Count / totalWorkforce;
        }

        public void StartStrike()
        {
            isOnStrike = true;
            Debug.Log($"STRIKE! {unionName} has stopped work.");
            // Logic to disable buildings where members work
        }
    }

    public class Strike
    {
        public Union ownerUnion;
        public List<World.Building> affectedBuildings = new List<World.Building>();
        public float economicLossPerMonth = 0f;
    }
}
