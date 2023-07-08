using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCanvas : MonoBehaviour
{
    public delegate void OnPause(bool cutAvailable);
    public static event OnPause onPause;

    [Header("Labels")]
    [SerializeField] private GameObject backLabel;
    [SerializeField] private GameObject gameOverLabel;
    [SerializeField] private GameObject completeLabel;
    [SerializeField] private GameObject settingsLabel;
    [SerializeField] private GameObject informationLabel;
    [SerializeField] private GameObject blurImage;

    [Header("Stars")]
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStar;
    [SerializeField] private GameObject threeStar;

    [Space]
    [SerializeField] private TextMeshProUGUI cuttsText;

    [Space]
    [SerializeField] private Animator pause_Load;

    private int cutts;

    private void Awake()
    {
        backLabel.SetActive(false);
        completeLabel.SetActive(false);
        gameOverLabel.SetActive(false);
        settingsLabel.SetActive(false);
        informationLabel.SetActive(false);
        blurImage.SetActive(false);

        //stars
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);

        //animation
        //pause_Load.SetBool("Load", true);
    }
    private void Start()
    {
        //GameOver
        Result.onGameOverLevel += GameOverLabelOn;
        Destroyer.onGameOverLevel += GameOverLabelOn;
        LaserObstacle.onGameOverLevel += GameOverLabelOn;
    }
    private void OnDestroy()
    { 
        //GameOver
        Result.onGameOverLevel -= GameOverLabelOn;
        Destroyer.onGameOverLevel -= GameOverLabelOn;
        LaserObstacle.onGameOverLevel += GameOverLabelOn;
    }

    #region Labels
    public void BackLabelOn()
    {
        backLabel.SetActive(true);
        blurImage.SetActive(true);
        Gamepause();
    }
    public void SettingsLabelOn()
    {
        settingsLabel.SetActive(true);
        blurImage.SetActive(true);
        Gamepause();
    }
    public void GameOverLabelOn()
    {
        gameOverLabel.SetActive(true);
        blurImage.SetActive(true);
        Gamepause();
    }
    public void InformationLabelOn()
    {
        informationLabel.SetActive(true);
        blurImage.SetActive(true);
        Gamepause();
    }
    public void LabelsOff()
    {
        Time.timeScale = 1;

        backLabel.SetActive(false);
        settingsLabel.SetActive(false);
        informationLabel.SetActive(false);
        blurImage.SetActive(false);
        onPause?.Invoke(true);
    }
    private void Gamepause()
    {
        Time.timeScale = 0;
        onPause?.Invoke(false);
    }
    public void LevelCompleteLabelOn(int finalCase)
    {
        completeLabel.SetActive(true);
        blurImage.SetActive(true);
        switch (finalCase)
        {
            case 1:
                //Debug.Log("1 Star");
                oneStar.SetActive(true);
                break;
            case 2:
                //Debug.Log("2 Star");
                oneStar.SetActive(true);
                twoStar.SetActive(true);
                break;
            case 3:
                //Debug.Log("3 Star");
                oneStar.SetActive(true);
                twoStar.SetActive(true);
                threeStar.SetActive(true);
                break;
        }
    }
    #endregion

    #region Cutts_And_Start
    public void UpdateNumberOfCuttesText(int currentCutts)
    {
        cutts = currentCutts;
        cuttsText.text = currentCutts.ToString();
    }
    #endregion
}
