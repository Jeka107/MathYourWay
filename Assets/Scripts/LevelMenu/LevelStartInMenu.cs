using UnityEngine;
using TMPro;

public class LevelStartInMenu : MonoBehaviour
{
    public delegate void OnLevelClick(string level);
    public static event OnLevelClick onLevelClick;

    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject levelLock;
    [SerializeField] private GameObject startLevel;
    [SerializeField] private GameObject perfectComplete;
    public void UnLockLevel()
    {
        startLevel.SetActive(true);
        levelLock.SetActive(false);
    }
    public void PerfectComplete()
    {
        perfectComplete.SetActive(true);
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
