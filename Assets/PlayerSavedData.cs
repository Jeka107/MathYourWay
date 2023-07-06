using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerSavedData
{
    public bool status;
    public int stars;

    public PlayerSavedData()
    {
        stars = 0;
    }

    public PlayerSavedData(int _stars)
    {
        stars = _stars;
    }
}
