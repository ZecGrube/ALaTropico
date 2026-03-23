using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    public enum FormationType { Line, Column, Square, Skirmish, Strategic }

    [CreateAssetMenu(fileName = "NewUnitType", menuName = "CaudilloBay/Military/UnitType")]
    public class UnitType : ScriptableObject
    {
        public string unitId;
        public string displayName;
        public Sprite icon;
        public GameObject prefab;

        [Header("Stats")]
        public float baseAttack = 10f;
        public float baseDefense = 10f;
        public float baseSpeed = 5f;
        public float baseUpkeep = 5f;

        [Header("Cost")]
        public List<ResourceCost> recruitmentCost;
        public int requiredPopulation = 1;
    }
}
