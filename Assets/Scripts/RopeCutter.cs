using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeCutter : MonoBehaviour
{
    public delegate void OnCut();
    public static event OnCut onCut;

    [SerializeField] private GameObject cutTrailPrefab;
    [SerializeField] private float maxTime;

    private Rigidbody2D rb;
    private GameObject currentCutTrail;
    private Vector2 touchPosition;
    private RaycastHit2D hit2;
    private bool cutAvailable=true;
    private bool ropeCut;
    private float startTime;
    private float endTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        InputManager.playerInput.Player.Touch.started += StartCutting;
        InputManager.playerInput.Player.TouchPosition.performed += UpdateCut;
        InputManager.playerInput.Player.Touch.canceled += StopCutting;

        LevelCanvas.onPause += CutAvailable;
    }
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        InputManager.playerInput.Player.Touch.started -= StartCutting;
        InputManager.playerInput.Player.TouchPosition.performed -= UpdateCut;
        InputManager.playerInput.Player.Touch.canceled -= StopCutting;

        LevelCanvas.onPause -= CutAvailable;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Piece")
        {
            Transform hook = collision.transform.parent;

            foreach (Transform piece in hook)
            {
                if (piece.tag == "Piece")
                    Destroy(piece.gameObject);
            }
            ropeCut = true;
        }
    }
    private void UpdateCut(InputAction.CallbackContext ctx)
    {
        if (cutAvailable)
        {
            touchPosition = ctx.ReadValue<Vector2>();
            rb.position = Camera.main.ScreenToWorldPoint(touchPosition);

            hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPosition), Vector2.zero);
            endTime = (float)ctx.time;

            if ((endTime - startTime) > maxTime)
            {
                DestroyCutTrail();
            }
        }
    }
    private void StartCutting(InputAction.CallbackContext ctx)
    {
        if (cutAvailable)
        {
            //Debug.Log("Cut Started");

            startTime = (float)ctx.startTime;
            StartCoroutine(CreateCutTrail());
        }
    }
    IEnumerator CreateCutTrail()
    {
        yield return new WaitForFixedUpdate();
        currentCutTrail = Instantiate(cutTrailPrefab, transform);
    }
    private void StopCutting(InputAction.CallbackContext ctx)
    {
        if (cutAvailable)
        {
            DestroyCutTrail();

            //Debug.Log("Cut Finished");

            if (ropeCut)
            {
                onCut?.Invoke();
                ropeCut = false;
            }

        }
    }
    private void DestroyCutTrail()
    {
        if (currentCutTrail != null)
        {
            currentCutTrail?.transform.SetParent(null);
            Destroy(currentCutTrail);
        }
    }
    private void CutAvailable(bool _cutAvailable)
    {
        cutAvailable = _cutAvailable;
    }
}
