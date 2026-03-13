namespace CaudilloBay.Core
{
    /// <summary>
    /// Represents a world object that can be harvested for resources.
    /// </summary>
    public interface IResourceNode : IPlaceable
    {
        string ResourceId { get; }
        float AmountLeft { get; set; }
        bool IsExhausted { get; }
        float Harvest(float harvestRate);
    }
}
