using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace CaudilloBay.Core
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        [Header("Scene Names")]
        public string mainMenuScene = "MainMenu";
        public string gameScene = "GameScene";

        [Header("UI References")]
        public GameObject loadingOverlay;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadSceneCoroutine(mainMenuScene));
        }

        public void StartNewGame()
        {
            // Reset game managers here if needed
            StartCoroutine(LoadSceneCoroutine(gameScene));
        }

        public void QuitGame()
        {
            Debug.Log("Exiting Game...");
            Application.Quit();
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            if (loadingOverlay != null)
                loadingOverlay.SetActive(true);

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

            while (!op.isDone)
            {
                // Update progress bar here if one exists
                yield return null;
            }

            if (loadingOverlay != null)
                loadingOverlay.SetActive(false);
        }
    }
}
