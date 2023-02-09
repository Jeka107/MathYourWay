using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSimple
{
    public string level;
    public string points;

    public LevelSimple()
    {

    }

    public LevelSimple(string level, string points)
    {
        this.level = level;
        this.points = points;
    }
}
