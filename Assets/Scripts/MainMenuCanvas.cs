using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject settingsLabel;
    [SerializeField] private GameObject blurImage;

    private void Awake()
    {
        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }
    public void SettingsLabelOn()
    {
        settingsLabel.SetActive(true);
        blurImage.SetActive(true);
    }
    public void SettingsLabelOff()
    {
        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }
}
