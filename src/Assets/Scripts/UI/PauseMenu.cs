using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pausePanel;
        public Slider timeScaleSlider;

        private bool isPaused = false;

        private void Update()
        {
            if (InputManager.Instance.GetActionDown(GameAction.Pause))
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : timeScaleSlider.value;
        }

        public void SaveGame() => SaveSystem.Instance?.SaveGame();
        public void ExitToMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
