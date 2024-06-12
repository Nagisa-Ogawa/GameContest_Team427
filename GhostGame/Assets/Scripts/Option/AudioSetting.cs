using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Slider���g�p���邽�߂ɕK�v
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] Slider MasterSlider;//���ʒ����p�X���C�_�[
    [SerializeField] Slider BGMSlider;//���ʒ����p�X���C�_�[
    [SerializeField] Slider SESlider;//���ʒ����p�X���C�_�[

    static float MasterVolume = 0.0f; //�ۑ��p
    static float BGMVolume = 0.0f; //�ۑ��p
    static float SEVolume = 0.0f; //�ۑ��p

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