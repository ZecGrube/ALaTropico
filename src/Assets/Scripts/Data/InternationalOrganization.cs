using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "New International Organization", menuName = "CaudilloBay/International Organization")]
    public class InternationalOrganization : ScriptableObject
    {
        public string orgId;
        public string orgName;
        [TextArea] public string description;

        public List<Politics.SuperpowerType> members = new List<Politics.SuperpowerType>();

        public float legitimacyThreshold = 40f;
        public float yearlyDues = 1000f;

        public List<ModifierData> membershipBonuses = new List<ModifierData>();
        public List<string> specificDemands = new List<string>(); // e.g. "NoDeathPenalty"
    }
}
