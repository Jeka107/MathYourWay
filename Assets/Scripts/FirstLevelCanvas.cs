using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevelCanvas : MonoBehaviour
{
    public delegate void OnClick();
    public static event OnClick onClick;

    public delegate void OnPause(bool cutAvailable);
    public static event OnPause onPause;

    [SerializeField] private GameObject informationLabel;
    [SerializeField] private GameObject blurImage;

    private SaveDataManager saveDataManager;
    private bool soundEffectStatus;
    private Animator animator;

    private void Awake()
    {
        saveDataManager = FindObjectOfType<SaveDataManager>();
        if (saveDataManager != null)
            soundEffectStatus = saveDataManager.LoadSettingsData();

        informationLabel.SetActive(true);
        blurImage.SetActive(true);
        animator = informationLabel.GetComponent<Animator>();

        StartCoroutine(PauseGame());
    }
    IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        onPause?.Invoke(false);
    }
    public void informationLabelOff()
    {
        Time.timeScale = 1;

        if (soundEffectStatus)
        {
            Debug.Log(1);
            onClick?.Invoke();
        }
        animator.SetBool("CloseInformation", true);
        blurImage.SetActive(false);
        onPause?.Invoke(true);

        StartCoroutine(DeActiveInformationLable());
    }
    IEnumerator DeActiveInformationLable()
    {
        yield return new WaitForSeconds(0.5f);
        informationLabel.SetActive(false);
    }
}
