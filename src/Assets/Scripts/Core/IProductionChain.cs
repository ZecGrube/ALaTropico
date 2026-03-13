using System.Collections.Generic;

namespace CaudilloBay.Core
{
    /// <summary>
    /// Defines a production chain (e.g., Sugar -> Rum).
    /// </summary>
    public interface IProductionChain
    {
        string Id { get; }
        string BuildingId { get; }
        List<ResourceQuantity> Inputs { get; }
        List<ResourceQuantity> Outputs { get; }
        float CycleTime { get; }
        int RequiredWorkers { get; }
        bool IsActive { get; set; }
    }

    public struct ResourceQuantity
    {
        public string ResourceId;
        public float Quantity;
    }
}
