using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using CaudilloBay.Data;

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

        [Header("Active Game State")]
        public GameMode currentMode = GameMode.Sandbox;
        public CampaignMission activeMission;
        public SandboxSettings sandboxSettings;

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

        public void StartCampaignMission(CampaignMission mission)
        {
            currentMode = GameMode.Campaign;
            activeMission = mission;
            StartCoroutine(LoadAndSetupGame());
        }

        public void StartSandboxGame(SandboxSettings settings)
        {
            currentMode = GameMode.Sandbox;
            sandboxSettings = settings;
            StartCoroutine(LoadAndSetupGame());
        }

        private IEnumerator LoadAndSetupGame()
        {
            yield return StartCoroutine(LoadSceneCoroutine(gameScene));

            // Setup Island
            World.IslandGenerator generator = UnityEngine.Object.FindAnyObjectByType<World.IslandGenerator>();
            if (generator != null)
            {
                if (currentMode == GameMode.Campaign) generator.SetupFromMission(activeMission);
                else generator.SetupFromSandbox(sandboxSettings);

                generator.GenerateIsland();
            }

            // Setup Mission Manager
            if (currentMode == GameMode.Campaign && CampaignManager.Instance != null)
            {
                CampaignManager.Instance.LoadMission(activeMission);
            }
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
                yield return null;
            }

            if (loadingOverlay != null)
                loadingOverlay.SetActive(false);
        }
    }
}
