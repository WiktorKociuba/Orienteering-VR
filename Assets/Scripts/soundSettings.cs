using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class soundSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer audioMixer;
    void Start()
    {
        float volume;
        audioMixer.GetFloat("Volume", out volume);
        volumeSlider.value = Mathf.Pow(10, volume / 20);
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
    }
    public void OnVolumeChange(float value)
    {
        float volume = Mathf.Log10(value) * 20;
        audioMixer.SetFloat("Volume", volume);
    }

}
