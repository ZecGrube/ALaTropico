using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class Heir
    {
        public string name;
        public int age;
        public float charisma;
        public float intelligence;
        public float military;
        public float cruelty;

        public float loyaltyToRuler = 100f;

        [System.Serializable]
        public struct FactionSupportEntry
        {
            public FactionType faction;
            public float value;
        }
        public List<FactionSupportEntry> factionSupportList = new List<FactionSupportEntry>();

        public void GenerateRandomStats()
        {
            charisma = Random.Range(20, 90);
            intelligence = Random.Range(20, 90);
            military = Random.Range(20, 90);
            cruelty = Random.Range(0, 50);
        }

        public void UpdateFactionSupport(List<FactionData> factions)
        {
            foreach (var f in factions)
            {
                float support = 50f;
                // Add logic based on traits and faction type
                if (f.type == FactionType.Capitalists) support += (intelligence * 0.2f);
                if (f.type == FactionType.Nationalists) support += (military * 0.2f);

                bool found = false;
                for (int i = 0; i < factionSupportList.Count; i++)
                {
                    if (factionSupportList[i].faction == f.type)
                    {
                        var entry = factionSupportList[i];
                        entry.value = support;
                        factionSupportList[i] = entry;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    factionSupportList.Add(new FactionSupportEntry { faction = f.type, value = support });
                }
            }
        }
    }
}
