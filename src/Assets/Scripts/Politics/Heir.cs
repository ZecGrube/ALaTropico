using UnityEngine;
using System;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public enum HeirGender
    {
        Male,
        Female,
        NonBinary
    }

    public enum SecretTrait
    {
        None,
        Conspirator,
        ForeignAsset,
        Opportunist,
        Reformist
    }

    [Serializable]
    public class Heir
    {
        public string heirName;
        public int age;
        public HeirGender gender;
        public Sprite portrait;

        [Range(0f, 100f)] public float charisma;
        [Range(0f, 100f)] public float cruelty;
        [Range(0f, 100f)] public float intelligence;
        [Range(0f, 100f)] public float military;

        [Range(0f, 100f)] public float loyaltyToRuler;
        [Range(0f, 100f)] public float jealousy;
        public bool isAlive = true;

        public List<SecretTrait> secretTraits = new List<SecretTrait>();
        public Dictionary<FactionType, float> factionSupport = new Dictionary<FactionType, float>();

        public void GenerateRandomStats()
        {
            charisma = UnityEngine.Random.Range(20f, 95f);
            cruelty = UnityEngine.Random.Range(5f, 95f);
            intelligence = UnityEngine.Random.Range(20f, 95f);
            military = UnityEngine.Random.Range(10f, 95f);
            loyaltyToRuler = UnityEngine.Random.Range(30f, 90f);
            jealousy = UnityEngine.Random.Range(5f, 85f);

            if (secretTraits.Count == 0 && UnityEngine.Random.Range(0f, 100f) < 35f)
            {
                secretTraits.Add((SecretTrait)UnityEngine.Random.Range(1, Enum.GetValues(typeof(SecretTrait)).Length));
            }
        }

        public void UpdateSupportBasedOnFactions(List<FactionData> factions)
        {
            foreach (var faction in factions)
            {
                float baseAffinity = faction.loyalty;
                float personalAffinity = 0f;

                switch (faction.type)
                {
                    case FactionType.Nationalists:
                        personalAffinity = military * 0.35f + charisma * 0.2f;
                        break;
                    case FactionType.Religious:
                        personalAffinity = (100f - cruelty) * 0.3f + charisma * 0.25f;
                        break;
                    case FactionType.Capitalists:
                        personalAffinity = intelligence * 0.3f + charisma * 0.2f;
                        break;
                    case FactionType.Criminals:
                        personalAffinity = cruelty * 0.35f + jealousy * 0.2f;
                        break;
                    default:
                        personalAffinity = (charisma + intelligence + military) / 6f;
                        break;
                }

                float support = Mathf.Clamp((baseAffinity * 0.45f) + personalAffinity, 0f, 100f);
                if (factionSupport.ContainsKey(faction.type)) factionSupport[faction.type] = support;
                else factionSupport.Add(faction.type, support);
            }
        }

        public float GetAverageSupport()
        {
            if (factionSupport.Count == 0) return 0f;
            float total = 0f;
            foreach (var entry in factionSupport) total += entry.Value;
            return total / factionSupport.Count;
        }
    }
}
