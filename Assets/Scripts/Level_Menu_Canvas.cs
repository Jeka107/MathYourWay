using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Menu_Canvas : MonoBehaviour
{
    [SerializeField] private GameObject startLevelLabel;
    [SerializeField] private GameObject settingsLabel;
    [SerializeField] private GameObject blurImage;
    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float waitBeforeLoad;

    private string currentLevel;

    private void Awake()
    {
        startLevelLabel.SetActive(false);
        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }
    private void Start()
    {
        LevelStartInMenu.onLevelClick += StartLevelLabelOn;
    }
    private void OnDestroy()
    {
        LevelStartInMenu.onLevelClick -= StartLevelLabelOn;
    }

    //Start_Level
    private void StartLevelLabelOn(string level)
    {
        audioSource.Play();

        currentLevel = level;
        startLevelLabel.SetActive(true);
        blurImage.SetActive(true);
        startLevelLabel.GetComponentInChildren<TextMeshProUGUI>().text = "level "+"\n" + level;
    }
    public void SettingsLabelOn()
    {
        audioSource.Play();

        settingsLabel.SetActive(true);
        blurImage.SetActive(true);
    }
    public void LabelsOff()
    {
        audioSource.Play();

        startLevelLabel.SetActive(false); 
        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }

    public void LoadLevel()
    {
        audioSource.Play();
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(waitBeforeLoad);

        SceneManager.LoadScene("Level " + currentLevel);
    }
}
