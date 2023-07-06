using UnityEngine;
using System.Collections.Generic;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField] SaveDataManager saveDataManager;

    private List<PlayerSavedData> playerSavedData;
    private int currentLevelNumber = 1;

    private void Awake()
    {
        PlaceNumberOnLevels();

        playerSavedData = saveDataManager.LoadData();
        if(playerSavedData!=null)
            CheckLevelsStatus();
    }

    private void PlaceNumberOnLevels()
    {
        foreach (Transform level in transform)
        {
            LevelStartInMenu levelStart = level.GetComponent<LevelStartInMenu>();

            if (levelStart != null)
            {
                levelStart.PlaceNumber(currentLevelNumber);
                currentLevelNumber++;
            }
        }
    }
    private void CheckLevelsStatus()
    {
        for(int i=0;i< playerSavedData?.Count;i++)
        {
            LevelStartInMenu level = transform.GetChild(i).GetComponent<LevelStartInMenu>();
            level.UnLockLevel();

            if(playerSavedData[i].stars==3)
            {
                level.PerfectComplete();
            }
        }
    }
}
