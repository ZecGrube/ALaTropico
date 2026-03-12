using UnityEngine;
using CaudilloBay.Politics;

namespace CaudilloBay.Data
{
    public class SamplePoliticalContent : MonoBehaviour
    {
        // This is a helper to demonstrate the data. In Unity, these would be .asset files.
        public Decree foodSubsidies;
        public Decree extraTaxes;

        public void SetupSampleData()
        {
            foodSubsidies = ScriptableObject.CreateInstance<Decree>();
            foodSubsidies.decreeName = "Food Subsidies";
            foodSubsidies.mandateCost = 10;
            foodSubsidies.peasantLoyaltyEffect = 15f;
            foodSubsidies.taxModifier = -0.1f;

            extraTaxes = ScriptableObject.CreateInstance<Decree>();
            extraTaxes.decreeName = "Extra Taxes";
            extraTaxes.mandateCost = 5;
            extraTaxes.peasantLoyaltyEffect = -20f;
            extraTaxes.taxModifier = 0.2f;
        }
    }
}
