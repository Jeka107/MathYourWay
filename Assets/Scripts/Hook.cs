using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject pieceOfRope;
    [SerializeField] private Vector2 firstPieceScale;
    [SerializeField] private int ropeLenght;
    [SerializeField] private Vector2 anchor;
   
    [HideInInspector] public GameObject lastPieceOfRope;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        CreateRope();
    }
    public void CreateRope()
    {
        GameObject currentPieceOfRope;
        Rigidbody2D currentRb;
        HingeJoint2D joint2D;

        //First piece on the hook and connect to the hook.
        currentPieceOfRope = Instantiate(pieceOfRope, transform);
        //currentPieceOfRope.transform.localScale = firstPieceScale;
        currentPieceOfRope.GetComponent<HingeJoint2D>().connectedBody = rb;
        currentRb = currentPieceOfRope.GetComponent<Rigidbody2D>();

        for (int i = 1; i < ropeLenght; i++)
        {
            currentPieceOfRope = Instantiate(pieceOfRope, transform);
            joint2D = currentPieceOfRope.GetComponent<HingeJoint2D>();
            joint2D.connectedBody = currentRb;
            //joint2D.connectedAnchor = anchor;
            currentRb = currentPieceOfRope.GetComponent<Rigidbody2D>();
        }
        lastPieceOfRope = currentPieceOfRope;
    }
}
