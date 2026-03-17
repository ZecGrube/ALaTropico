using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Core
{
    public class PatentManager : MonoBehaviour
    {
        public static PatentManager Instance { get; private set; }

        public List<string> activePatents = new List<string>();
        public float monthlyRoyaltyIncome = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterPatent(string techName)
        {
            if (activePatents.Contains(techName)) return;

            activePatents.Add(techName);
            float royalty = Random.Range(50f, 200f);
            monthlyRoyaltyIncome += royalty;
            Debug.Log($"Patent registered for {techName}. Expected monthly royalties: ${royalty}");
        }

        public void SellPatent(string techName, float price)
        {
            if (activePatents.Contains(techName))
            {
                activePatents.Remove(techName);
                if (Economy.EconomyManager.Instance != null)
                {
                    Economy.EconomyManager.Instance.AddFunds(price);
                }
                Debug.Log($"Sold patent rights for {techName} for ${price}");
            }
        }

        public void UpdateMonthly()
        {
            if (Economy.EconomyManager.Instance != null)
            {
                Economy.EconomyManager.Instance.AddFunds(monthlyRoyaltyIncome);
            }
        }
    }
}
