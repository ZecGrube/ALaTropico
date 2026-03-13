using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class TooltipSystem : MonoBehaviour
    {
        public static TooltipSystem Instance { get; private set; }

        public GameObject tooltipBox;
        public Text headerText;
        public Text contentText;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            tooltipBox.SetActive(false);
        }

        public void Show(string contentKey, string headerKey = "")
        {
            if (LocalizationManager.Instance != null)
            {
                headerText.text = LocalizationManager.Instance.GetText(headerKey);
                contentText.text = LocalizationManager.Instance.GetText(contentKey);
            }
            else
            {
                headerText.text = headerKey;
                contentText.text = contentKey;
            }
            tooltipBox.SetActive(true);
        }

        public void Hide()
        {
            tooltipBox.SetActive(false);
        }

        private void Update()
        {
            if (tooltipBox.activeSelf)
            {
                tooltipBox.transform.position = Input.mousePosition;
            }
        }
    }
}
