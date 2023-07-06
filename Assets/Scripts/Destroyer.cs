using UnityEngine;
using System;
using Enums;

public class Destroyer : MonoBehaviour
{
    public delegate void OnGameOverLevel();
    public static event OnGameOverLevel onGameOverLevel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Enum.IsDefined(typeof(DESTROYER_GAMEOBJECT), collision.gameObject.tag))
        {
            if (collision.gameObject.tag == DESTROYER_GAMEOBJECT.Ball.ToString())
            {
                Debug.Log("Game Over!!!!!");
                onGameOverLevel?.Invoke();
            }
            Destroy(collision.gameObject);
        }
    }
}
