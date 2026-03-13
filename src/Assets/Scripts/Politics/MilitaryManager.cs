using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class MilitaryManager : MonoBehaviour
    {
        public static MilitaryManager Instance { get; private set; }

        public float totalMilitaryStrength = 0f;
        public float baseStrength = 0f;
        public float trainingLevel = 50f;
        public float readiness = 100f;
        public float armyLoyalty = 50f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddBarracksStrength(float strength)
        {
            baseStrength += strength;
            UpdateMilitaryStrength();
        }

        public void RemoveBarracksStrength(float strength)
        {
            baseStrength -= strength;
            UpdateMilitaryStrength();
        }

        public void UpdateMilitaryStrength()
        {
            totalMilitaryStrength = baseStrength * (trainingLevel / 100f) * (readiness / 100f);
        }

        public void UpdateArmyLoyalty(float delta)
        {
            armyLoyalty = Mathf.Clamp(armyLoyalty + delta, 0, 100);
        }
    }
}
