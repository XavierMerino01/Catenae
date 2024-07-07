using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    //private Resolution[] resolutions;
    private List<Resolution> resolutions = new List<Resolution>
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1024, height = 768 }
    };
    private PostProcessProfile postProcessProfile;
    private ColorGrading colorGrading;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);

        brightnessSlider.onValueChanged.AddListener(SetBrightness);

        // resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Count; i++)
        {
            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(resolutionOption);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        var postProcessVolume = FindObjectOfType<PostProcessVolume>();
        postProcessProfile = postProcessVolume.profile;
        postProcessProfile.TryGetSettings(out colorGrading);

        //SetupNavigation();
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void SetBrightness(float brightness)
    {
        if (colorGrading != null)
        {
            colorGrading.postExposure.value = brightness;
        }
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    private void SetupNavigation()
    {
        Navigation volumeNav = new Navigation();
        volumeNav.mode = Navigation.Mode.Explicit;
        volumeNav.selectOnUp = fullscreenToggle;
        volumeNav.selectOnDown = brightnessSlider;
        volumeSlider.navigation = volumeNav;

        // Set up navigation for brightnessSlider
        Navigation brightnessNav = new Navigation();
        brightnessNav.mode = Navigation.Mode.Explicit;
        brightnessNav.selectOnUp = volumeSlider;
        brightnessNav.selectOnDown = resolutionDropdown;
        brightnessSlider.navigation = brightnessNav;

        // Set up navigation for resolutionDropdown
        Navigation resolutionNav = new Navigation();
        resolutionNav.mode = Navigation.Mode.Explicit;
        resolutionNav.selectOnUp = brightnessSlider;
        resolutionNav.selectOnDown = fullscreenToggle;
        resolutionDropdown.navigation = resolutionNav;

        // Set up navigation for fullscreenToggle
        Navigation fullscreenNav = new Navigation();
        fullscreenNav.mode = Navigation.Mode.Explicit;
        fullscreenNav.selectOnUp = resolutionDropdown;
        fullscreenNav.selectOnDown = volumeSlider;
        fullscreenToggle.navigation = fullscreenNav;

        var dropdownTemplate = resolutionDropdown.template.GetComponent<RectTransform>();
        if (dropdownTemplate != null)
        {
            var dropdownElements = dropdownTemplate.GetComponentsInChildren<Selectable>(true);
            for (int i = 0; i < dropdownElements.Length; i++)
            {
                Navigation dropdownElementNav = new Navigation();
                dropdownElementNav.mode = Navigation.Mode.Explicit;

                if (i > 0)
                {
                    dropdownElementNav.selectOnUp = dropdownElements[i - 1];
                }
                if (i < dropdownElements.Length - 1)
                {
                    dropdownElementNav.selectOnDown = dropdownElements[i + 1];
                }

                dropdownElements[i].navigation = dropdownElementNav;
            }
        }
    }
}
