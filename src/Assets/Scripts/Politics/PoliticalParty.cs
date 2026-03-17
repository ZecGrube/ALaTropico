using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class PoliticalParty
    {
        public string partyName;
        public FactionType coreIdeology;
        public FactionLeader leader;
        public float supportRate; // 0-100
        public bool isBanned = false;

        public List<string> agenda = new List<string>();
    }

    public class ParliamentManager : MonoBehaviour
    {
        public static ParliamentManager Instance { get; private set; }

        public List<PoliticalParty> parties = new List<PoliticalParty>();
        public int totalSeats = 100;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RunParliamentarySession()
        {
            Debug.Log("Parliament is in session...");
            CalculateSeatDistribution();
        }

        private void CalculateSeatDistribution()
        {
            // Simplified: seats proportional to support rate of non-banned parties
            float totalSupport = 0f;
            foreach(var p in parties) if(!p.isBanned) totalSupport += p.supportRate;

            foreach(var p in parties)
            {
                if(p.isBanned) continue;
                int seats = Mathf.RoundToInt((p.supportRate / totalSupport) * totalSeats);
                Debug.Log($"{p.partyName} holds {seats} seats.");
            }
        }
    }
}
