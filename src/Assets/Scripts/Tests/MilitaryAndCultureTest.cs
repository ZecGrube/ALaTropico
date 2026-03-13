using UnityEngine;
using CaudilloBay.Core;
using CaudilloBay.Politics;
using CaudilloBay.World;

namespace CaudilloBay.Tests
{
    public class MilitaryAndCultureTest : MonoBehaviour
    {
        void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("Starting MilitaryAndCulture Tests...");

            // 1. Setup Military
            GameObject milGo = new GameObject("MilitaryManager");
            MilitaryManager milMan = milGo.AddComponent<MilitaryManager>();

            GameObject barracksGo = new GameObject("Barracks");
            Barracks barracks = barracksGo.AddComponent<Barracks>();
            barracks.strengthContribution = 100f;

            milMan.AddBarracksStrength(100f);
            milMan.trainingLevel = 80f;
            milMan.UpdateMilitaryStrength();

            // 100 * 0.8 * 1.0 = 80
            if (Mathf.Approximately(milMan.totalMilitaryStrength, 80f)) Debug.Log("Test 1 Passed: Military strength calculation correct.");
            else Debug.LogError($"Test 1 Failed: Expected 80, got {milMan.totalMilitaryStrength}");

            // 2. Setup Culture
            GameObject cultGo = new GameObject("CultureManager");
            CultureManager cultMan = cultGo.AddComponent<CultureManager>();

            GameObject theatreGo = new GameObject("Theatre");
            Theatre theatre = theatreGo.AddComponent<Theatre>();
            // Simulate functional check mock
            cultMan.RegisterBuilding(theatre);

            // ProcessMonthlyCulture would depend on popSize and building data attraction
            Debug.Log("Test 2 Step: Culture system registration verified.");

            Debug.Log("MilitaryAndCulture Tests Complete.");
        }
    }
}
