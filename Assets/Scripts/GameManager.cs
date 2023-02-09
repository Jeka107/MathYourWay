using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
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
