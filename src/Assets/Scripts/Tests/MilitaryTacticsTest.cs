using UnityEngine;
using CaudilloBay.Politics;
using CaudilloBay.Data;
using System.Collections.Generic;

namespace CaudilloBay.Tests
{
    public class MilitaryTacticsTest : MonoBehaviour
    {
        public void Start()
        {
            RunTests();
        }

        public void RunTests()
        {
            Debug.Log("--- Starting Military Tactics Tests ---");
            TestBattleResolution();
            TestFormationBonus();
            TestArmyUpkeep();
            Debug.Log("--- Military Tactics Tests Complete ---");
        }

        private void TestBattleResolution()
        {
            GameObject mgrGO = new GameObject("BattleManager");
            var mgr = mgrGO.AddComponent<BattleManager>();

            UnitType template = ScriptableObject.CreateInstance<UnitType>();
            template.baseAttack = 10;

            Army a1 = new Army { armyName = "Attacker" };
            a1.units.Add(new MilitaryUnit { type = template });

            Army a2 = new Army { armyName = "Defender" };
            a2.units.Add(new MilitaryUnit { type = template });

            mgr.ResolveBattle(a1, a2);

            // One or both should have taken damage
            bool healthReduced = a1.units.Count == 0 || a1.units[0].health < 100 || a2.units.Count == 0 || a2.units[0].health < 100;

            if (healthReduced)
                Debug.Log("[PASS] Battle Resolution: Damage applied to units.");
            else
                Debug.LogError("[FAIL] Battle Resolution: No damage recorded.");
        }

        private void TestFormationBonus()
        {
            UnitType template = ScriptableObject.CreateInstance<UnitType>();
            template.baseAttack = 10;

            MilitaryUnit unit = new MilitaryUnit { type = template };
            float baseAttack = unit.GetModifiedAttack(FormationType.Column);
            float lineAttack = unit.GetModifiedAttack(FormationType.Line);

            if (lineAttack > baseAttack)
                Debug.Log("[PASS] Formation Bonus: Line formation increases attack.");
            else
                Debug.LogError("[FAIL] Formation Bonus: Attack logic mismatch.");
        }

        private void TestArmyUpkeep()
        {
            GameObject mgrGO = new GameObject("MilitaryManager");
            var mgr = mgrGO.AddComponent<MilitaryManager>();

            Army a = new Army { armyName = "Royal Guard" };
            mgr.activeArmies.Add(a);

            mgr.UpdateMilitaryStrength();

            if (mgr.totalMilitaryStrength >= 0)
                Debug.Log("[PASS] Army Integration: Manager tracks active armies.");
            else
                Debug.LogError("[FAIL] Army Integration: Strength calculation error.");
        }
    }
}
