namespace CaudilloBay.Core
{
    /// <summary>
    /// Represents a building that can be constructed by builders.
    /// </summary>
    public interface IBuildable : IPlaceable
    {
        float ConstructionTime { get; }
        float CurrentProgress { get; set; }
        bool IsConstructed { get; }
        void AddProgress(float delta);
    }
}
