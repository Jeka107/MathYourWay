using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip ballMerged;
    [SerializeField] private AudioClip slash;

    private void Start()
    {
        LevelCanvas.onClick += ClickButton;
        GameManager.onClick += ClickButton;
        Ball.onBallMerged += BallMerged;
        RopeCutter.onSlash += Slash;
    }
    private void OnDestroy()
    {
        LevelCanvas.onClick -= ClickButton;
        GameManager.onClick -= ClickButton;
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
        audioSource.PlayOneShot(slash);
    }
}
