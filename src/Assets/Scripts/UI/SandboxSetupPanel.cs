using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;
using CaudilloBay.Data;

namespace CaudilloBay.UI
{
    public class SandboxSetupPanel : MonoBehaviour
    {
        public Dropdown sizeDropdown;
        public Dropdown richnessDropdown;
        public Slider moneySlider;

        public void OnStartClicked()
        {
            SandboxSettings settings = new SandboxSettings
            {
                size = (SandboxSettings.IslandSize)sizeDropdown.value,
                richness = (SandboxSettings.ResourceRichness)richnessDropdown.value,
                startingMoney = moneySlider.value
            };

            GameStateManager.Instance.StartSandboxGame(settings);
        }
    }
}
