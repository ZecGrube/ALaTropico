namespace CaudilloBay.Data
{
    public enum ObjectiveType { BuildBuilding, ReachPopulation, AccumulateWealth, SurviveTime, ReachLoyalty }

    [System.Serializable]
    public class CampaignObjectiveData
    {
        public ObjectiveType type;
        public string description;
        public float targetValue;
        public string targetId; // For BuildBuilding (BuildingId) or ReachLoyalty (FactionType)
    }

    public class CampaignObjective
    {
        public CampaignObjectiveData data;
        public float currentValue;
        public bool isComplete;

        public CampaignObjective(CampaignObjectiveData data)
        {
            this.data = data;
            this.currentValue = 0;
            this.isComplete = false;
        }
    }
}
