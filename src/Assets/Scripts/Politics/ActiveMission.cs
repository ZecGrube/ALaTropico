using UnityEngine;

namespace CaudilloBay.Politics
{
    [System.Serializable]
    public class ActiveMission
    {
        public GlobalMission template;
        public Agent assignedAgent;
        public float timeRemaining;

        public bool IsComplete => timeRemaining <= 0;

        public void Update(float deltaTime)
        {
            if (timeRemaining > 0)
                timeRemaining -= deltaTime;
        }
    }
}
