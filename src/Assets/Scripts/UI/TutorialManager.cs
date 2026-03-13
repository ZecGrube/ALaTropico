using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Data;

namespace CaudilloBay.UI
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        public GameObject tutorialPanel;
        public Text titleText;
        public Text descriptionText;
        public GameObject highlightOverlay;

        private List<TutorialStep> allSteps = new List<TutorialStep>();
        public HashSet<string> completedSteps = new HashSet<string>();
        private TutorialStep currentStep;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void StartTutorial(List<TutorialStep> steps)
        {
            allSteps = steps;
            ShowNextStep();
        }

        public void OnEvent(TutorialTrigger trigger)
        {
            if (currentStep != null && currentStep.trigger == trigger)
            {
                CompleteStep();
            }
        }

        private void ShowNextStep()
        {
            foreach (var step in allSteps)
            {
                if (!completedSteps.Contains(step.stepId))
                {
                    currentStep = step;
                    UpdateUI();
                    return;
                }
            }
            tutorialPanel.SetActive(false);
        }

        private void UpdateUI()
        {
            if (currentStep == null) return;

            titleText.text = Core.LocalizationManager.Instance.GetText(currentStep.titleKey);
            descriptionText.text = Core.LocalizationManager.Instance.GetText(currentStep.descriptionKey);
            tutorialPanel.SetActive(true);

            if (!string.IsNullOrEmpty(currentStep.highlightTag))
            {
                GameObject target = GameObject.FindWithTag(currentStep.highlightTag);
                if (target != null)
                {
                    highlightOverlay.SetActive(true);
                    highlightOverlay.transform.position = target.transform.position;
                }
            }
            else
            {
                highlightOverlay.SetActive(false);
            }
        }

        public void CompleteStep()
        {
            if (currentStep != null)
            {
                completedSteps.Add(currentStep.stepId);
                ShowNextStep();
            }
        }

        public void SkipTutorial()
        {
            tutorialPanel.SetActive(false);
            highlightOverlay.SetActive(false);
            currentStep = null;
        }
    }
}
