using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ResolveBattle(Army attacker, Army defender)
        {
            Debug.Log($"BATTLE: {attacker.armyName} attacks {defender.armyName}");

            float attackPower = attacker.GetTotalPower();
            float defensePower = defender.GetTotalPower();

            // Formation bonuses (Rock-Paper-Scissors)
            if (attacker.currentFormation == Data.FormationType.Line && defender.currentFormation == Data.FormationType.Column)
                attackPower *= 1.5f;
            if (attacker.currentFormation == Data.FormationType.Square && defender.currentFormation == Data.FormationType.Line)
                defensePower *= 1.5f;

            float totalPower = attackPower + defensePower;
            float winChance = attackPower / totalPower;

            bool attackerWins = Random.value < winChance;

            ApplyLosses(attacker, defender, winChance);

            if (attackerWins)
            {
                Debug.Log($"{attacker.armyName} is victorious!");
                // Defender retreats or is destroyed
            }
            else
            {
                Debug.Log($"{defender.armyName} repelled the attack!");
            }
        }

        private void ApplyLosses(Army attacker, Army defender, float differential)
        {
            float attackerLossRate = (1f - differential) * 0.2f;
            float defenderLossRate = differential * 0.2f;

            foreach (var unit in attacker.units) unit.TakeDamage(100f * attackerLossRate);
            foreach (var unit in defender.units) unit.TakeDamage(100f * defenderLossRate);

            // Clean up dead units
            attacker.units.RemoveAll(u => u.health <= 0);
            defender.units.RemoveAll(u => u.health <= 0);
        }
    }
}
