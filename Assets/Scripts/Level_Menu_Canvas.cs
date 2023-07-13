using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Menu_Canvas : MonoBehaviour
{
    public delegate void OnClick();
    public static event OnClick onClick;

    [SerializeField] private GameObject startLevelLabel;
    [SerializeField] private GameObject blurImage;

    [Space]
    [Header("Settings")]
    [SerializeField] private Settings settings;
    
    [Space]
    [SerializeField] private float waitBeforeLoad;

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
        PlaySoundEffect();

        currentLevel = level;
        startLevelLabel.SetActive(true);
        blurImage.SetActive(true);
        startLevelLabel.GetComponentInChildren<TextMeshProUGUI>().text = "level "+"\n" + level;
    }
    
    public void LabelsOff()
    {
        PlaySoundEffect();

        startLevelLabel.SetActive(false); 
        blurImage.SetActive(false);
    }

    public void LoadLevel()
    {
        PlaySoundEffect();
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(waitBeforeLoad);

        SceneManager.LoadScene("Level " + currentLevel);
    }
    private void PlaySoundEffect()
    {
        if (settings.GetSoundEffectStatus())
            onClick?.Invoke();
    }
}
