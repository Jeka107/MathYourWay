using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int number;

    [Header("Level Stars")]
    [SerializeField] private int threeStartsCutts;
    [SerializeField] private int twoStartsCutts;

    [Space]
    [SerializeField] private LevelCanvas levelCanvas;
    
    private SaveDataManager saveDataManager;
    private int currentCutts; //count numbers of cutts.
    
    private void Start()
    {
        saveDataManager = FindAnyObjectByType<SaveDataManager>();
        RopeCutter.onCut += UpdateNumberOfCuttes;
        Result.onCompleteLevel += CheckCompleteStars;
    }
    private void OnDestroy()
    {
        RopeCutter.onCut -= UpdateNumberOfCuttes;
        Result.onCompleteLevel -= CheckCompleteStars;
    }
    private void UpdateNumberOfCuttes()
    {
        currentCutts++;
        levelCanvas.UpdateNumberOfCuttesText(currentCutts);//update cutts in display.
    }
    private void CheckCompleteStars()
    {
        int finalCase;

        if (currentCutts <= threeStartsCutts)
        {
            finalCase = 3;
        }
        else if (currentCutts <= twoStartsCutts)
        {
            finalCase = 2;
        }
        else
        {
            finalCase = 1;
        }
        saveDataManager.UpdateDataList(number,finalCase);
        levelCanvas.LevelCompleteLabelOn(finalCase);
    }

    #region SceneManagment
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameUnPause();
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameUnPause();
    }
    public void BackToLevelMenu()
    {
        SceneManager.LoadScene("LevelsMenu");
        GameUnPause();
    }
    #endregion
    private void GameUnPause()
    {
        Time.timeScale = 1;
    }
}
