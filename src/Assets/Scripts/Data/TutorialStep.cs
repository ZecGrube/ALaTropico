using UnityEngine;

namespace CaudilloBay.Data
{
    public enum TutorialTrigger { Start, BuildingPlaced, MenuOpened, MonthPassed }

    [CreateAssetMenu(fileName = "New Tutorial Step", menuName = "Caudillo Bay/Tutorial Step")]
    public class TutorialStep : ScriptableObject
    {
        public string stepId;
        public string titleKey;
        public string descriptionKey;
        public TutorialTrigger trigger;
        public string highlightTag; // Tag of the UI object to highlight
        public bool blockInput = false;
    }
}
