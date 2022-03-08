using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace RPG.Settings
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] AudioMixer musicMixer;
        [SerializeField] AudioMixer effectsMixer;

        [SerializeField] Slider musicSlider;
        [SerializeField] Slider effectsSlider;
        [SerializeField] Toggle fullScreenToggle;
        [SerializeField] TMP_Dropdown resolutionDropdown;

        List<Resolution> resolutions;


        private void Start() {
            float volume;
            musicMixer.GetFloat("volume", out volume);
            musicSlider.value = volume;

            effectsMixer.GetFloat("volume", out volume);
            effectsSlider.value = volume;

            fullScreenToggle.isOn = Screen.fullScreen;
       
            EnableResolutionChange();        
        }

        private void EnableResolutionChange()
        {
            resolutions = new List<Resolution>(Screen.resolutions);
            resolutions = resolutions.Where(p => ((float)p.width / 16) - ((float)p.height / 9) == 0)
                            .Reverse()
                            .ToList();

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for(int i = 0; i < resolutions.Count; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if(resolutions[i].width == Screen.currentResolution.width && 
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution (int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetMusicVolume(float volume)
        {
            musicMixer.SetFloat("volume", volume);
        }

        public void SetEffectsVolume(float volume)
        {
            effectsMixer.SetFloat("volume", volume);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
    }
}
