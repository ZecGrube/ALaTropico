using CaudilloBay.Data;

namespace CaudilloBay.Core
{
    public interface IProducer
    {
        float ProductionRate { get; }
        bool CanProduce { get; }
        void ProduceCycle();
    }
}
