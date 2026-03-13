using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CaudilloBay.UI
{
    public class OptionsPanel : MonoBehaviour
    {
        [Header("Audio")]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        [Header("Graphics")]
        public Dropdown resolutionDropdown;
        public Toggle fullscreenToggle;

        private Resolution[] resolutions;

        private void Start()
        {
            LoadSettings();
            InitializeResolutionDropdown();
        }

        public void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat("MasterVolume", volume);
            // Apply volume to AudioMixer here
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        public void SetResolution(int index)
        {
            Resolution res = resolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
        }

        private void InitializeResolutionDropdown()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
            resolutionDropdown.RefreshShownValue();
        }

        private void LoadSettings()
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        }

        public void SaveAllSettings()
        {
            PlayerPrefs.Save();
        }
    }
}
