using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerSavedData
{
    public int stars;
    public bool sound;

    public PlayerSavedData()
    {
        stars = 0;
        sound = false;
    }

    public PlayerSavedData(int _stars)
    {
        stars = _stars;
    }
}

[Serializable]
public class PlayerSavedSettings
{
    public bool soundEffectStatus;

    public PlayerSavedSettings()
    {
        soundEffectStatus = false;
    }
    public PlayerSavedSettings(bool _soundEffectStatus)
    {
        soundEffectStatus = _soundEffectStatus;
    }

}
