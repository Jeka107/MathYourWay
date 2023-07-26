using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public delegate void OnClick();
    public static event OnClick onClick;

    public delegate void OnPause(bool cutAvailable);
    public static event OnPause onPause;

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

        saveDataManager = FindObjectOfType<SaveDataManager>();
        if(saveDataManager!=null)
            soundEffectStatus = saveDataManager.LoadSettingsData();
        SoundEffectStatus();
    }
    public void SettingsLabelOn()
    {
        if(soundEffectStatus)
            onClick?.Invoke();

        settingsLabel.SetActive(true);
        blurImage.SetActive(true);

        Time.timeScale = 0;
        onPause?.Invoke(false);
    }
    public void SettingsLabelOff()
    {
        if (soundEffectStatus)
            onClick?.Invoke();

        settingsLabel.SetActive(false);
        blurImage.SetActive(false);

        Time.timeScale = 1;
        onPause?.Invoke(true);
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
