using CaudilloBay.Data;

namespace CaudilloBay.Core
{
    public interface IConsumer
    {
        float ConsumptionRate { get; }
        void ConsumeCycle();
    }
}
