using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class graphicsSettings : MonoBehaviour
{
    public TMP_Dropdown resolution;
    public TMP_Dropdown quality;

    private Resolution[] resolutions;
    void Start()
    {
        SetupResolutionDropdown();
        SetupQualityDropdown();
        LoadSettings();
    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();
    }
    public void SetResolution()
    {
        Resolution resolutionValue = resolutions[resolution.value];
        Screen.SetResolution(resolutionValue.width, resolutionValue.height, Screen.fullScreen);
        print("resolution" + resolutionValue.width + "x" + resolutionValue.height);
        SaveSettings();
    }
    void SetupQualityDropdown()
    {
        quality.ClearOptions();
        List<string> options = new List<string>(QualitySettings.names);
        quality.AddOptions(options);
        quality.value = QualitySettings.GetQualityLevel();
        quality.RefreshShownValue();
    }
    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(quality.value);
        SaveSettings();
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("resolution", resolution.value);
        PlayerPrefs.SetInt("quality", quality.value);
        PlayerPrefs.Save();
    }
    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("resolution"))
        {
            resolution.value = PlayerPrefs.GetInt("resolution");
            SetResolution();
        }
        if (PlayerPrefs.HasKey("quality"))
        {
            quality.value = PlayerPrefs.GetInt("quality");
            SetQuality();
        }
    }
}
