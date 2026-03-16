using System.Collections.Generic;

namespace CaudilloBay.AI
{
    [System.Serializable]
    public class Family
    {
        public int familyId;
        public List<Citizen> members = new List<Citizen>();
        public float totalWealth;

        public void UpdateWealth()
        {
            totalWealth = 0;
            foreach (var m in members) totalWealth += m.personalWealth;
        }

        public bool IsNuclear() => members.Count <= 4;
    }
}
