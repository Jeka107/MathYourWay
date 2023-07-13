using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject blurImage;

    [Space]
    [Header("Settings")]
    [SerializeField] private Settings settings;
    
    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float waitBeforeLoad;

    [Space]
    [Header("Sound Effect")]
    [SerializeField] private GameObject soundEffectOn;
    [SerializeField] private GameObject soundEffectOff;

    private void Awake()
    {
        blurImage.SetActive(false);
    }

    public void StartGame()
    {
        PlaySoundEffect();
        StartCoroutine(LoadNextScene());
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(waitBeforeLoad);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void PlaySoundEffect()
    {
        if (settings.GetSoundEffectStatus())
            audioSource.Play();
    }
}
