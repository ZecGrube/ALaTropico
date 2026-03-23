using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class MilitaryUnit
    {
        public Data.UnitType type;
        public float health = 100f;
        public float morale = 100f;
        public float experience = 0f;

        public float GetModifiedAttack(Data.FormationType formation)
        {
            float bonus = 1.0f;
            if (formation == Data.FormationType.Line) bonus = 1.2f;
            return type.baseAttack * bonus * (morale / 100f) * (1.0f + experience * 0.1f);
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            morale -= amount * 0.5f;
        }
    }

    [System.Serializable]
    public class General
    {
        public string name;
        public float leadership = 50f;
        public float tacticalSkill = 50f;
        public float loyalty = 50f;
    }

    [System.Serializable]
    public class Army
    {
        public string armyName;
        public List<MilitaryUnit> units = new List<MilitaryUnit>();
        public Data.FormationType currentFormation = Data.FormationType.Line;
        public General commander;
        public Vector2 gridPosition;

        public float supplyLevel = 100f;
        public bool isHostile = false;

        public float GetTotalPower()
        {
            float power = 0;
            foreach (var unit in units) power += unit.GetModifiedAttack(currentFormation);
            return power * (supplyLevel / 100f);
        }
    }
}
