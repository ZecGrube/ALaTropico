namespace CaudilloBay.World
{
    public class PoliceHQ : Building
    {
        public float antiCorruptionPower = 8f;

        protected override void CompleteConstruction()
        {
            base.CompleteConstruction();
            if (CaudilloBay.Politics.CorruptionManager.Instance != null)
            {
                CaudilloBay.Politics.CorruptionManager.Instance.RegisterAntiCorruptionSource(GetAntiCorruptionKey(), antiCorruptionPower);
            }
        }

        private void OnDestroy()
        {
            if (CaudilloBay.Politics.CorruptionManager.Instance != null)
            {
                CaudilloBay.Politics.CorruptionManager.Instance.UnregisterAntiCorruptionSource(GetAntiCorruptionKey());
            }
        }

        private string GetAntiCorruptionKey()
        {
            return $"PoliceHQ_{GetInstanceID()}";
        }
    }
}
