using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color ballColor;
    [SerializeField] public int value;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public string arithmeticAction;

    private Arithmetic arithmetic;

    private void Start()
    {
        arithmetic = GetComponent<Arithmetic>();
    }

    public void UpdateBallInfo(Color _ballColor, int _value)
    {
        ballColor = _ballColor;
        value = _value;
        text.text = value.ToString();

        Invoke("UpdateLayerTag", 1f);//after creation update layer and tags.
    }

    public void UpdateBall(Color _ballColor, int newValue)
    {
        ballColor = _ballColor;
        gameObject.GetComponent<SpriteRenderer>().color = ballColor;
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        arithmeticAction = "Def";
        value = newValue;
        text.text = newValue.ToString();

        gameObject.layer = LayerMask.NameToLayer("Ball");
        gameObject.tag = "Ball";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "ColoredBall")
        {
            //add sound of ball collision.

            var otherBall = collision.gameObject.GetComponent<Ball>();
            var newValue = arithmetic.ArithmeticAction(otherBall.arithmeticAction, value, otherBall.value);

            collision.gameObject.GetComponent<Ball>().UpdateBall(ballColor, newValue);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "ColoredBall")
        {
            //add sound of ball collision.

            var otherBall = collision.gameObject.GetComponent<Ball>();
            var newValue = arithmetic.ArithmeticAction(otherBall.arithmeticAction, value, otherBall.value);

            collision.gameObject.GetComponent<Ball>().UpdateBall(ballColor, newValue);
            Destroy(gameObject);
        }
    }
    private void UpdateLayerTag()
    {
        if (arithmeticAction != "Def")
        {
            gameObject.layer = LayerMask.NameToLayer("ColoredBall");
            gameObject.tag = "ColoredBall";
        }
        else
            gameObject.layer = LayerMask.NameToLayer("Ball");
    }
}
