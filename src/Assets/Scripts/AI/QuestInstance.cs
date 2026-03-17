using UnityEngine;

namespace CaudilloBay.AI
{
    [System.Serializable]
    public class QuestInstance
    {
        public Data.Quest template;
        public int currentProgress;
        public bool isComplete;

        public QuestInstance(Data.Quest t)
        {
            template = t;
            currentProgress = 0;
            isComplete = false;
        }

        public void AddProgress(int amount)
        {
            if (isComplete) return;
            currentProgress += amount;
            if (currentProgress >= template.requiredAmount)
            {
                currentProgress = template.requiredAmount;
                isComplete = true;
            }
        }

        public float GetProgressPercent() => (float)currentProgress / template.requiredAmount;
    }
}
