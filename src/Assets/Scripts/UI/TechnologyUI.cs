using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CaudilloBay.Data;
using CaudilloBay.World;

namespace CaudilloBay.UI
{
    public class TechnologyUI : MonoBehaviour
    {
        public GameObject techNodePrefab;
        public Transform nodeContainer;
        public Text rpText;
        public Image progressFill;

        private void Update()
        {
            if (TechnologyManager.Instance != null)
            {
                rpText.text = $"Research Points: {TechnologyManager.Instance.currentResearchPoints:F0}";

                if (TechnologyManager.Instance.currentResearch != null)
                {
                    progressFill.fillAmount = TechnologyManager.Instance.researchProgress;
                }
                else
                {
                    progressFill.fillAmount = 0;
                }
            }
        }

        public void RefreshTree()
        {
            // Logic to clear and rebuild the tech tree visual nodes
        }
    }
}
