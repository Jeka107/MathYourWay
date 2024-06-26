using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ball : MonoBehaviour
{
    public delegate void OnBallMerged();
    public static event OnBallMerged onBallMerged;

    [SerializeField] private Color ballColor;
    [SerializeField] public int value;
    [SerializeField] private TextMeshPro text;
    [SerializeField] public string arithmeticAction;

    private List<Rigidbody2D> rbLastPieces;
    private Settings settings;
    private Arithmetic arithmetic;
    private float y;

    private void Awake()
    {
        rbLastPieces = new List<Rigidbody2D>();
        settings = FindAnyObjectByType<Settings>();
    }
    private void Start()
    {
        y = transform.position.y;
        arithmetic = GetComponent<Arithmetic>();
    }
    private void Update()
    {
        if (gameObject.GetComponents<HingeJoint2D>() != null)
        {
            foreach (HingeJoint2D joint in gameObject.GetComponents<HingeJoint2D>())
            {
                if (joint.connectedBody == null)
                    Destroy(joint);
            }
        }
        
        if (gameObject.GetComponent<HingeJoint2D>() == null)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
    public void UpdateBallInfo(Color _ballColor, int _value)
    {
        ballColor = _ballColor;
        value = _value;
        text.text = value.ToString();

        Invoke("UpdateLayerTag", 1f);//after creation update layer and tags.
    }

    public void UpdateBall(Color _ballColor, int newValue, List<Rigidbody2D> rbLastPieces)
    {
        ballColor = _ballColor;
        gameObject.GetComponent<SpriteRenderer>().color = ballColor;
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        arithmeticAction = "Def";
        value = newValue;
        text.text = newValue.ToString();

        gameObject.layer = LayerMask.NameToLayer("Ball");
        gameObject.tag = "Ball";
        gameObject.GetComponent<Transform>().eulerAngles = Vector3.zero;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        
        if (rbLastPieces.Count != 0)
        {
            UpdateNumHingeJoint(rbLastPieces);
        }
    }

    //Connect to the remaining ropes
    private void UpdateNumHingeJoint(List<Rigidbody2D> rbLastPieces)
    {      
        HingeJoint2D hingeJoint2D;

        for (int i = 0; i < rbLastPieces.Count; i++)
        {
            hingeJoint2D = gameObject.AddComponent<HingeJoint2D>();
            hingeJoint2D.connectedBody = rbLastPieces[i];
            hingeJoint2D.autoConfigureConnectedAnchor = false;
            hingeJoint2D.connectedAnchor = gameObject.GetComponentInParent<Hook>().anchor;
        }      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.tag != "ColoredBall")
        {
            if (collision.transform.tag == "ColoredBall")
            {
                PlaySoundEffect();

                var otherBall = collision.gameObject.GetComponent<Ball>();
                var newValue = arithmetic.ArithmeticAction(otherBall.arithmeticAction, value, otherBall.value);

                //Check if connected to ropes.
                foreach(HingeJoint2D joint in gameObject.GetComponents<HingeJoint2D>())
                {
                    
                    if (joint!=null)
                    {  
                        rbLastPieces.Add(joint.connectedBody);
                    }
                }

                collision.gameObject.GetComponent<Ball>().UpdateBall(ballColor, newValue, rbLastPieces);
                Destroy(gameObject);
            }
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
    private void PlaySoundEffect()
    {
        if(settings.GetSoundEffectStatus())
            onBallMerged?.Invoke();
    }
}
