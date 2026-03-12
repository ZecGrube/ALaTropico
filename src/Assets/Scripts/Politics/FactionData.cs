using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class FactionData
    {
        public FactionType type;
        public string displayName;
        [Range(0, 100)]
        public float loyalty = 50f;
        public float supportBase = 0f;
    }
}
