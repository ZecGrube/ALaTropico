using UnityEngine;

namespace CaudilloBay.Politics
{
    [CreateAssetMenu(fileName = "New Faction Leader", menuName = "Caudillo Bay/Faction Leader")]
    public class FactionLeader : ScriptableObject
    {
        public string leaderName;
        public Sprite portrait;
        public PersonalityType personality;
    }
}
