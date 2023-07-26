using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    [SerializeField] private float activeTime;

    private bool currentState = false;

    private void Awake()
    {
        StartCoroutine(LaserActivation());
    }
    IEnumerator LaserActivation()
    {
        while(true)
        {
            if(currentState)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                currentState = false;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                currentState = true;
            }
            yield return new WaitForSeconds(activeTime);
        }
    }
}
