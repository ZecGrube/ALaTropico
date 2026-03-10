namespace CaudilloBay.Core
{
    /// <summary>
    /// Represents a basic game resource (e.g., Bananas, Rum, Gold).
    /// </summary>
    public interface IResource
    {
        string Id { get; }
        string DisplayName { get; }
        ResourceCategory Category { get; }
        float BaseValue { get; }
        float Weight { get; }
    }

    public enum ResourceCategory
    {
        Agriculture,
        Forestry,
        Mineral,
        Intermediate,
        Final,
        Luxury,
        Strategic
    }
}
