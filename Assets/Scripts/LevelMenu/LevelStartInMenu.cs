using UnityEngine;
using TMPro;

public class LevelStartInMenu : MonoBehaviour
{
    public delegate void OnLevelClick(string level);
    public static event OnLevelClick onLevelClick;

    [SerializeField] private TextMeshProUGUI levelNumberText;

    public void PlaceNumber(int levelNumber)
    {
        levelNumberText.text = levelNumber.ToString();
    }
    public void LevelClick()
    {
        onLevelClick?.Invoke(levelNumberText.text.ToString());
    }
}
