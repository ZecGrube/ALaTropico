using System.Collections.Generic;
using CaudilloBay.World;
using CaudilloBay.Politics;

namespace CaudilloBay.Economy
{
    public enum CorporationType
    {
        StateMonopoly,
        Private,
        Foreign
    }

    public enum IndustryType
    {
        Agriculture,
        Mining,
        Industry,
        Tourism,
        Transport
    }

    [System.Serializable]
    public class Corporation
    {
        public string corporationName;
        public CorporationType type;
        public IndustryType industry;
        public List<Building> ownedBuildings = new List<Building>();
        public float treasury;
        public int totalShares;
        public float sharePrice;

        public float stateOwnershipPercent; // 0 to 100
        public float privateOwnershipPercent; // 0 to 100
        public float foreignOwnershipPercent; // 0 to 100

        [System.Serializable]
        public struct FactionInfluenceEntry
        {
            public FactionType faction;
            public float value;
        }
        public List<FactionInfluenceEntry> lobbyingPowerList = new List<FactionInfluenceEntry>();

        public Corporation(string name, CorporationType type, IndustryType industry)
        {
            this.corporationName = name;
            this.type = type;
            this.industry = industry;
            this.totalShares = 1000;
            this.sharePrice = 10f;

            InitializeOwnership();
            InitializeLobbying();
        }

        private void InitializeOwnership()
        {
            switch (type)
            {
                case CorporationType.StateMonopoly:
                    stateOwnershipPercent = 100;
                    break;
                case CorporationType.Private:
                    privateOwnershipPercent = 100;
                    break;
                case CorporationType.Foreign:
                    foreignOwnershipPercent = 100;
                    break;
            }
        }

        private void InitializeLobbying()
        {
            lobbyingPowerList.Clear();
            foreach (FactionType faction in System.Enum.GetValues(typeof(FactionType)))
            {
                lobbyingPowerList.Add(new FactionInfluenceEntry { faction = faction, value = 0 });
            }
        }

        public float CalculateMonthlyProfit()
        {
            float profit = 0;
            foreach (var building in ownedBuildings)
            {
                if (building != null && building.health > 0)
                {
                    // Logic for building profit will be integrated here or called from EconomyManager
                    // For now, assume building has a way to report its monthly performance
                }
            }
            return profit;
        }

        public void PayDividends(float amount)
        {
            if (treasury >= amount)
            {
                treasury -= amount;
                // Distribute to state treasury if state owns shares
                float stateShare = amount * (stateOwnershipPercent / 100f);
                EconomyManager.Instance.AddFunds(stateShare);

                // Other shares go to 'abstract' private/foreign owners for now
            }
        }

        public void IssueShares(int amount)
        {
            totalShares += amount;
            treasury += amount * sharePrice;
            // Dilute ownership logic would go here
        }

        public void BuyShares(int amount, float price)
        {
            // Logic for buying shares from the market
        }

        public void Lobby(FactionType faction, float amount)
        {
            if (treasury >= amount)
            {
                treasury -= amount;
                FactionManager.Instance.AddLoyalty(faction, amount / 100f); // Simple scaling

                for (int i = 0; i < lobbyingPowerList.Count; i++)
                {
                    if (lobbyingPowerList[i].faction == faction)
                    {
                        var entry = lobbyingPowerList[i];
                        entry.value += amount;
                        lobbyingPowerList[i] = entry;
                        break;
                    }
                }
            }
        }

        public void UpdateSharePrice()
        {
            // Simple valuation: assets + treasury / shares
            float assetValue = ownedBuildings.Count * 500f; // Placeholder
            sharePrice = (assetValue + treasury) / totalShares;
        }
    }
}
