//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.Rendering.PostProcessing;
//using System.Collections.Generic;

//public class OptionsMenu : MonoBehaviour
//{
//    [Header("UI Elements")]
//    public Slider volumeSlider;
//    public Slider brightnessSlider;
//    public TMP_Dropdown resolutionDropdown;
//    public Toggle fullscreenToggle;

//    private Resolution[] resolutions;
//    private PostProcessProfile postProcessProfile;

//    private void Start()
//    {
//        // Setup volume slider
//        volumeSlider.onValueChanged.AddListener(SetVolume);

//        // Setup brightness slider
//        brightnessSlider.onValueChanged.AddListener(SetBrightness);

//        // Setup resolution dropdown
//        resolutions = Screen.resolutions;
//        resolutionDropdown.ClearOptions();
//        int currentResolutionIndex = 0;
//        List<string> options = new List<string>();
//        for (int i = 0; i < resolutions.Length; i++)
//        {
//            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;
//            options.Add(resolutionOption);
//            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
//            {
//                currentResolutionIndex = i;
//            }
//        }
//        resolutionDropdown.AddOptions(options);
//        resolutionDropdown.value = currentResolutionIndex;
//        resolutionDropdown.RefreshShownValue();
//        resolutionDropdown.onValueChanged.AddListener(SetResolution);

//        // Setup fullscreen toggle
//        fullscreenToggle.isOn = Screen.fullScreen;
//        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

//        // Initialize PostProcessingProfile for brightness control
//        postProcessProfile = GetComponent<PostProcessVolume>().profile;
//    }

//    private void SetVolume(float volume)
//    {
//        AudioListener.volume = volume;
//    }

//    private void SetBrightness(float brightness)
//    {
//        if (postProcessProfile.TryGetSettings(out ColorGrading colorGrading))
//        {
//            colorGrading.postExposure.value = brightness;
//        }
//    }

//    private void SetResolution(int resolutionIndex)
//    {
//        Resolution resolution = resolutions[resolutionIndex];
//        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
//    }

//    private void SetFullscreen(bool isFullscreen)
//    {
//        Screen.fullScreen = isFullscreen;
//    }
//}
