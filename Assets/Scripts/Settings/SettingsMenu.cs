using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Slider volumeSlider;

    private void Start()
    {
        float saved = VolumeSettings.SavedVolume;

        if (volumeSlider != null)
            volumeSlider.SetValueWithoutNotify(saved);

        if (VolumeSettings.Instance == null && mainMixer != null)
            mainMixer.SetFloat("volume", saved);
    }

    public void ResetToDefaults()
    {
        SetVolume(VolumeSettings.MaxDb);

        if (volumeSlider != null)
            volumeSlider.SetValueWithoutNotify(VolumeSettings.MaxDb);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetVolume(float volume)
    {
        if (VolumeSettings.Instance != null)
            VolumeSettings.Instance.Save(volume);
        else if (mainMixer != null)
            mainMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
