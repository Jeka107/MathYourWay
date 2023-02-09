using System.Collections.Generic;
using UnityEngine;

public class GameElements : MonoBehaviour
{
    [SerializeField] private List<Element> elements;

    private GameObject currentBall;
    
    private void Start()
    {
        CreateElements();
    }

    private void CreateElements()
    {
        HingeJoint2D hingeJoint2D;
        Hook currentHook=null;

       foreach(Element element in elements)
       {
            CreateBall(element);

            foreach (Hook hook in element.hooks)
            {
                currentHook = hook;
                hingeJoint2D =currentBall.AddComponent<HingeJoint2D>();
                hingeJoint2D.connectedBody = hook.lastPieceOfRope.GetComponent<Rigidbody2D>();
                hingeJoint2D.autoConfigureConnectedAnchor = false;
                hingeJoint2D.connectedAnchor = Vector2.zero;
            }
            currentBall.transform.SetParent(currentHook.transform);
       }
    }
    private void CreateBall(Element element)
    {
        Ball thisBall;

        currentBall = Instantiate(element.ballInfo.ball);
        thisBall = currentBall.GetComponent<Ball>();

        thisBall.UpdateBallInfo(element.ballInfo.ball.GetComponent<SpriteRenderer>().color, element.ballInfo.value);
    }
    
}
