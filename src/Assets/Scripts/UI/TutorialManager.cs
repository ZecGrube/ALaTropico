using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CaudilloBay.UI
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        public GameObject tutorialPanel;
        public Text tutorialText;
        private Queue<string> steps = new Queue<string>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void AddStep(string message)
        {
            steps.Enqueue(message);
            if (!tutorialPanel.activeSelf) ShowNextStep();
        }

        public void ShowNextStep()
        {
            if (steps.Count > 0)
            {
                tutorialText.text = steps.Dequeue();
                tutorialPanel.SetActive(true);
            }
            else
            {
                tutorialPanel.SetActive(false);
            }
        }
    }
}
