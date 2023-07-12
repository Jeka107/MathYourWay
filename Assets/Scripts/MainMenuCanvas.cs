using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject settingsLabel;
    [SerializeField] private GameObject blurImage;

    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float waitBeforeLoad;

    private void Awake()
    {
        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }
    public void StartGame()
    {
        audioSource.Play();
        StartCoroutine(LoadNextScene());
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(waitBeforeLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SettingsLabelOn()
    {
        audioSource.Play();
        settingsLabel.SetActive(true);
        blurImage.SetActive(true);
    }
    public void SettingsLabelOff()
    {
        audioSource.Play();
        settingsLabel.SetActive(false);
        blurImage.SetActive(false);
    }
}
