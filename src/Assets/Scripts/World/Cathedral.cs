namespace CaudilloBay.World
{
    public class Cathedral : ReligiousBuilding
    {
        public float attractivenessBonus = 20f;

        private void Awake()
        {
            influencePower = 30f;
        }
    }
}
