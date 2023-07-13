using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public delegate void OnClick();
    public static event OnClick onClick;

    [SerializeField] private GameObject settingsLabel;
    [SerializeField] private GameObject blurImage;

    [Space]
    [Header("Sound Effect")]
    [SerializeField] private GameObject soundEffectOn;
    [SerializeField] private GameObject soundEffectOff;

    private SaveDataManager saveDataManager;
    private bool soundEffectStatus;

    private void Awake()
    {
        settingsLabel.SetActive(false);

        saveDataManager = FindAnyObjectByType<SaveDataManager>();
        soundEffectStatus = saveDataManager.LoadSettingsData();
        SoundEffectStatus();
    }
    public void SettingsLabelOn()
    {
        if(soundEffectStatus)
            onClick?.Invoke();

        settingsLabel.SetActive(true);
        blurImage.SetActive(true);
    }
    public void SettingsLabelOff()
    {
        if (soundEffectStatus)
            onClick?.Invoke();

        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }

    private void SoundEffectStatus()
    {
        if (soundEffectStatus)
        {
            soundEffectOn.SetActive(true);
        }
        else
        {
            soundEffectOff.SetActive(true);
        }
    }
    public void SoundEffectButton()
    {
        if (soundEffectStatus)
        {
            soundEffectStatus = false;
            soundEffectOn.SetActive(false);
            soundEffectOff.SetActive(true);

            saveDataManager.SetSoundStatus(soundEffectStatus);
        }
        else
        {
            onClick?.Invoke();
            soundEffectStatus = true;
            soundEffectOff.SetActive(false);
            soundEffectOn.SetActive(true);

            saveDataManager.SetSoundStatus(soundEffectStatus);
        }
    }
    public bool GetSoundEffectStatus()
    {
        return soundEffectStatus;
    }
}