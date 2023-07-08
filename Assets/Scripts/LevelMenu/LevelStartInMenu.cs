using UnityEngine;
using TMPro;

public class LevelStartInMenu : MonoBehaviour
{
    public delegate void OnLevelClick(string level);
    public static event OnLevelClick onLevelClick;

    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject levelLock;
    [SerializeField] private GameObject startLevel;

    [Space]
    [Header("Stars")]
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;

    public void UnLockLevel()
    {
        startLevel.SetActive(true);
        levelLock.SetActive(false);

        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
    }
    public void StarsCompleted(int starsStatus)
    {
        switch (starsStatus)
        {
            case 1:
                star1.SetActive(true);
                break;
            case 2:
                star1.SetActive(true);
                star2.SetActive(true);
                break;
            case 3:
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
                break;
        }
    }
    public void PlaceNumber(int levelNumber)
    {
        levelNumberText.text = levelNumber.ToString();
    }
    public void LevelClick()
    {
        onLevelClick?.Invoke(levelNumberText.text.ToString());
    }
}
