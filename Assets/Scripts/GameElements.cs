using System.Collections.Generic;
using UnityEngine;

public class GameElements : MonoBehaviour
{
    [SerializeField] private List<Element> elements;

    private GameObject currentBall;
    private Vector2 anchor;
    private Hook currentHook;
    private GameObject lastPieceOfRope;

    private void Start()
    {
        anchor = new Vector2(0, -0.3f);
        CreateElements();
    }

    private void CreateElements()
    {
        HingeJoint2D hingeJoint2D;
        

       foreach(Element element in elements)
       {
            lastPieceOfRope = element.hooks[0].lastPieceOfRope;
            CreateBall(element);

            foreach (Hook hook in element.hooks)
            {
                if (hook != null)
                {
                    currentHook = hook;
                    hingeJoint2D = currentBall.AddComponent<HingeJoint2D>();
                    hingeJoint2D.connectedBody = hook.lastPieceOfRope.GetComponent<Rigidbody2D>();
                    hingeJoint2D.autoConfigureConnectedAnchor = false;
                    hingeJoint2D.connectedAnchor = anchor;
                }
            }
            currentBall.transform.SetParent(currentHook.transform);
       }
    }
    private void CreateBall(Element element)
    {
        Ball thisBall;

        currentBall = Instantiate(element.ballInfo.ball,lastPieceOfRope.transform);
        thisBall = currentBall.GetComponent<Ball>();

        thisBall.UpdateBallInfo(element.ballInfo.ball.GetComponent<SpriteRenderer>().color, element.ballInfo.value);
    }

}
