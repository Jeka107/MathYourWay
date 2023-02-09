using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCanvas : MonoBehaviour
{
    public delegate void OnPause(bool cutAvailable);
    public static event OnPause onPause;

    [Header("Level Stars")]
    [SerializeField] private int threeStartsCutts;
    [SerializeField] private int twoStartsCutts;

    [Header("Labels")]
    [SerializeField] private GameObject backLabel;
    [SerializeField] private GameObject gameOverLabel;
    [SerializeField] private GameObject completeLabel;
    [SerializeField] private GameObject settingsLabel;

    [Header("Stars")]
    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStar;
    [SerializeField] private GameObject threeStar;

    [Space]
    [SerializeField] private TextMeshProUGUI cuttsText;

    private int currentCutts;

    private void Awake()
    {
        backLabel.SetActive(false);
        completeLabel.SetActive(false);
        gameOverLabel.SetActive(false);
        settingsLabel.SetActive(false);

        //stars
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);
    }
    private void Start()
    {
        RopeCutter.onCut += UpdateNumberOfCuttes;
        Result.onCompleteLevel += CheckCompleteStars;

        //GameOver
        Result.onGameOverLevel += GameOverLabelOn;
        Destroyer.onGameOverLevel += GameOverLabelOn;
    }
    private void OnDestroy()
    {
        RopeCutter.onCut -= UpdateNumberOfCuttes;
        Result.onCompleteLevel -= CheckCompleteStars;

        //GameOver
        Result.onGameOverLevel -= GameOverLabelOn;
        Destroyer.onGameOverLevel -= GameOverLabelOn;
    }

    #region Labels
    public void BackLabelOn()
    {
        backLabel.SetActive(true);
        Gamepause();
    }
    public void SettingsLabelOn()
    {
        settingsLabel.SetActive(true);
        Gamepause();
    }
    public void GameOverLabelOn()
    {
        gameOverLabel.SetActive(true);
        Gamepause();
    }
    public void LabelsOff()
    {
        Time.timeScale = 1;

        backLabel.SetActive(false);
        settingsLabel.SetActive(false);
        onPause?.Invoke(true);
    }
    private void Gamepause()
    {
        Time.timeScale = 0;
        onPause?.Invoke(false);
    }

    #endregion

    #region Cutts_And_Start
    private void UpdateNumberOfCuttes()
    {
        currentCutts++;
        cuttsText.text = currentCutts.ToString();
    }
    private void CheckCompleteStars()
    {
        completeLabel.SetActive(true);

        if (currentCutts <= threeStartsCutts)
        {
            Debug.Log("3 Star");
            oneStar.SetActive(true);
            twoStar.SetActive(true);
            threeStar.SetActive(true);
        }
        else if (currentCutts <= twoStartsCutts)
        {
            Debug.Log("2 Star");
            oneStar.SetActive(true);
            twoStar.SetActive(true);
        }
        else
        {
            Debug.Log("1 Star");
            oneStar.SetActive(true);
        }
    }
    #endregion
}
