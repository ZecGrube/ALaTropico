using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class ReligionUI : MonoBehaviour
    {
        public Text religiosityText;
        public Text cultLevelText;
        public Text leaderText;

        public void Update()
        {
            if (ReligionManager.Instance != null)
            {
                religiosityText.text = "Religiosity: " + ReligionManager.Instance.religiousInfluence.ToString("F1");
                if (ReligionManager.Instance.currentLeader != null)
                {
                    leaderText.text = "Leader: " + ReligionManager.Instance.currentLeader.agentName +
                                     " (Loyalty: " + ReligionManager.Instance.currentLeader.loyaltyToRegime.ToString("F0") + "%)";
                }
                else
                {
                    leaderText.text = "Leader: None";
                }
            }

            if (PersonalityCultManager.Instance != null)
            {
                cultLevelText.text = "Personality Cult: " + PersonalityCultManager.Instance.cultLevel.ToString("F1");
            }
        }
    }
}
