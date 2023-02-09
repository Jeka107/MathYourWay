using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
    private int currentLevelNumber = 1;

    private void Awake()
    {
        PlaceNumberOnLevels();
    }

    private void PlaceNumberOnLevels()
    {
        foreach(Transform level in transform)
        {
            LevelStartInMenu levelStart = level.GetComponent<LevelStartInMenu>();

            if (levelStart != null)
            {
                levelStart.PlaceNumber(currentLevelNumber);
                currentLevelNumber++;
            }
        }
    }
}
