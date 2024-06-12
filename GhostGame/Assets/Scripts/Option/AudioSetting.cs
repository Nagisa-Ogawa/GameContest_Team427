using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Sliderを使用するために必要
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] Slider MasterSlider;//音量調整用スライダー
    [SerializeField] Slider BGMSlider;//音量調整用スライダー
    [SerializeField] Slider SESlider;//音量調整用スライダー

    static float MasterVolume = 0.0f; //保存用
    static float BGMVolume = 0.0f; //保存用
    static float SEVolume = 0.0f; //保存用

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip testSound;

    void Awake()
    {
        MasterSlider.value = MasterVolume;
        BGMSlider.value = BGMVolume;
        SESlider.value = SEVolume;
    }

    public void SetMaster(float volume)
    {
        audioMixer.SetFloat("MasterVol", volume);
        BGMVolume = volume;
    }
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
        BGMVolume = volume;
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol", volume);
        SEVolume = volume;
        audioSource.PlayOneShot(testSound);
    }

}