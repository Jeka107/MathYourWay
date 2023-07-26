using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip ballMerged;
    [SerializeField] private AudioClip slash;

    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }
    private void Start()
    {
        //Click Button
        MainMenuCanvas.onClick += ClickButton;
        Level_Menu_Canvas.onClick += ClickButton;
        Settings.onClick += ClickButton;
        LevelCanvas.onClick += ClickButton;
        GameManager.onClick += ClickButton;
        FirstLevelCanvas.onClick += ClickButton;

        Ball.onBallMerged += BallMerged;
        RopeCutter.onSlash += Slash;
    }
    private void OnDestroy()
    {
        //Click Button
        MainMenuCanvas.onClick -= ClickButton;
        Level_Menu_Canvas.onClick -= ClickButton;
        Settings.onClick -= ClickButton;
        LevelCanvas.onClick -= ClickButton;
        GameManager.onClick -= ClickButton;
        FirstLevelCanvas.onClick -= ClickButton;

        Ball.onBallMerged -= BallMerged;
        RopeCutter.onSlash -= Slash;
    }
    private void ClickButton()
    {
        audioSource.priority = 130;
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(click);
    }
    private void BallMerged()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(ballMerged);
    }
    private void Slash()
    {
        audioSource.priority = 128;
        audioSource.volume = 1f;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(slash);
        }
        else
        {
            audioSource.Stop();
            audioSource.PlayOneShot(slash);
        }
    }
}
