using System.Collections.Generic;
using UnityEngine;
using CaudilloBay.World;

namespace CaudilloBay.Economy
{
    public class CorporationManager : MonoBehaviour
    {
        public static CorporationManager Instance { get; private set; }

        public List<Corporation> corporations = new List<Corporation>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Corporation CreateCorporation(string name, CorporationType type, IndustryType industry, List<Building> initialBuildings)
        {
            Corporation corp = new Corporation(name, type, industry);
            foreach (var building in initialBuildings)
            {
                if (building != null)
                {
                    building.ownerCorporation = corp;
                    corp.ownedBuildings.Add(building);
                }
            }
            corporations.Add(corp);
            return corp;
        }

        public void MergeCorporations(Corporation target, Corporation source)
        {
            if (target == null || source == null || target == source) return;

            foreach (var building in source.ownedBuildings)
            {
                building.ownerCorporation = target;
                target.ownedBuildings.Add(building);
            }

            target.treasury += source.treasury;
            // Simplified share merging
            target.totalShares += source.totalShares;

            corporations.Remove(source);
        }

        public void ProcessMonthlyFinances()
        {
            float taxRate = 0.15f; // Base corporate tax

            foreach (var corp in corporations)
            {
                float monthlyProfit = 0;
                foreach (var building in corp.ownedBuildings)
                {
                    if (building != null && building.health > 0)
                    {
                        // Profit logic handled in EconomyManager redirects to corp treasury
                        // Here we just update share price and handle dividends
                    }
                }

                // Pay taxes if private
                if (corp.type != CorporationType.StateMonopoly)
                {
                    float tax = corp.treasury * taxRate;
                    corp.treasury -= tax;
                    EconomyManager.Instance.AddFunds(tax);
                }
                else
                {
                    // State Monopolies transfer 90% of profit to treasury
                    float transfer = corp.treasury * 0.9f;
                    corp.treasury -= transfer;
                    EconomyManager.Instance.AddFunds(transfer);
                }

                corp.UpdateSharePrice();

                // Optional: Auto-pay dividends if treasury is high
                if (corp.treasury > 5000)
                {
                    corp.PayDividends(corp.treasury * 0.2f);
                }
            }
        }

        public void RemoveBuildingFromCorporation(Building building)
        {
            if (building.ownerCorporation != null)
            {
                building.ownerCorporation.ownedBuildings.Remove(building);
                building.ownerCorporation = null;
            }
        }

        public void OfferForeignInvestmentDeal()
        {
            Debug.Log("FOREIGN INVESTMENT OFFER: 'Global Fruit Inc' wants to build 3 new plantations for a fee of $10,000 and 10% tax break.");
            // UI trigger for player choice
        }

        public void NationalizeCorporation(Corporation corp)
        {
            Debug.Log($"URGENT: El Presidente has nationalized {corp.name}!");
            // Switch type - in real logic would match a specific enum
            // corp.type = CorporationType.StateMonopoly;

            // Relations with host country (if foreign) would plummet
            if (Politics.GlobalMapManager.Instance != null)
            {
                // Find host superpower and reduce relations
            }
        }
    }
}
