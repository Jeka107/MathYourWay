using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    public delegate void OnCompleteLevel();
    public static event OnCompleteLevel onCompleteLevel;

    public delegate void OnGameOverLevel();
    public static event OnGameOverLevel onGameOverLevel;

    [SerializeField] private TextMeshPro text;
    [SerializeField] private int result;

    private void Awake()
    {
        text.text = result.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (collision.gameObject.GetComponent<Ball>().value == result)
            {
                //Debug.Log("Level Complete");
                onCompleteLevel?.Invoke();
            }
            else
            {
                //Debug.Log("Game Over");
                onGameOverLevel?.Invoke();
            }
            Destroy(collision.gameObject); 
        }
    }
}
