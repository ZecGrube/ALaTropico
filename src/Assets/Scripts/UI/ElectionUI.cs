using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Politics;

namespace CaudilloBay.UI
{
    public class ElectionUI : MonoBehaviour
    {
        public GameObject electionPanel;
        public Text timerText;
        public Button holdElectionButton;
        public Button useFraudButton;

        private void Start()
        {
            electionPanel.SetActive(false);
            holdElectionButton.onClick.AddListener(() => OnHoldElection(false));
            useFraudButton.onClick.AddListener(() => OnHoldElection(true));
        }

        private void Update()
        {
            if (ElectionManager.Instance != null)
            {
                float timeRemaining = ElectionManager.Instance.nextElectionTime - Time.time;
                if (timeRemaining <= 0)
                {
                    timerText.text = "Election Day is Here!";
                    electionPanel.SetActive(true);
                }
                else
                {
                    timerText.text = $"Next Election in: {timeRemaining / 60f:F1} min";
                }
            }
        }

        private void OnHoldElection(bool useFraud)
        {
            ElectionManager.Instance.HoldElection(useFraud);
            electionPanel.SetActive(false);
        }
    }
}
