using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Menu_Canvas : MonoBehaviour
{
    [SerializeField] private GameObject startLevelLabel;
    [SerializeField] private GameObject blurImage;
    private string currentLevel;

    private void Awake()
    {
        startLevelLabel.SetActive(false);
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
        currentLevel = level;
        startLevelLabel.SetActive(true);
        blurImage.SetActive(true);
        startLevelLabel.GetComponentInChildren<TextMeshProUGUI>().text = "level " + level;
    }
    public void StartLevelLabelOff()
    {
        startLevelLabel.SetActive(false);
        blurImage.SetActive(false);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level " + currentLevel);
    }
}
