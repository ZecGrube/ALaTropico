using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;

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

        public void Show(string content, string header = "")
        {
            headerText.text = header;
            contentText.text = content;
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
