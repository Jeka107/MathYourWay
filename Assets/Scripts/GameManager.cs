using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public delegate void OnClick();
    public static event OnClick onClick;

    [Header("Level")]
    [SerializeField] private int number;

    [Header("Level Stars")]
    [SerializeField] private int threeStartsCutts;
    [SerializeField] private int twoStartsCutts;

    [Space]
    [SerializeField] private LevelCanvas levelCanvas;
    [SerializeField] private float waitBeforeLoadScene;


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
        saveDataManager?.UpdateDataList(number,finalCase);
        levelCanvas?.LevelCompleteLabelOn(finalCase);
    }

    #region SceneManagment
    public void RestartScene()
    {
        GameUnPause();
        PlaySoundEffect();
        StartCoroutine(Restart());
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(waitBeforeLoadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void LoadNextScene()
    {
        GameUnPause();
        PlaySoundEffect();
        StartCoroutine(LoadNext());
    }
    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(waitBeforeLoadScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void BackToLevelMenu()
    {
        GameUnPause();
        PlaySoundEffect();
        StartCoroutine(ToLevelMenu());
    }
    IEnumerator ToLevelMenu()
    {
        yield return new WaitForSeconds(waitBeforeLoadScene);
        SceneManager.LoadScene("LevelsMenu");
    }
    #endregion
    private void GameUnPause()
    {
        Time.timeScale = 1;
    }
    private void PlaySoundEffect()
    {
        onClick?.Invoke();
    }
}
