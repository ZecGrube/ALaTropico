using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Data
{
    [CreateAssetMenu(fileName = "NewEra", menuName = "CaudilloBay/Era")]
    public class Era : ScriptableObject
    {
        public string eraName;
        [TextArea] public string description;
        public int eraIndex;
        public Sprite icon;

        public List<Quest> mainQuests;
        public List<Quest> sideQuests;

        public Era nextEra;
    }
}
