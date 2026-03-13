using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    [System.Serializable]
    public class Sanction
    {
        public string sanctionId;
        public string issuerName;
        public string reason;
        public List<ModifierData> effects = new List<ModifierData>();
    }
}
