namespace CaudilloBay.Core
{
    /// <summary>
    /// Anything that has a footprint and a position in the global grid.
    /// </summary>
    public interface IPlaceable
    {
        string Id { get; }
        (int width, int height) Footprint { get; }
        (int x, int z) GridPosition { get; set; }
        bool IsFlippable { get; }
    }
}
