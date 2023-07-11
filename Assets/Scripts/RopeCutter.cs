using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeCutter : MonoBehaviour
{
    public delegate void OnCut();
    public static event OnCut onCut;

    public delegate void OnSlash();
    public static event OnSlash onSlash;

    [SerializeField] private GameObject cutTrailPrefab;
    [SerializeField] private float maxTime;
    [SerializeField] private float minTimeSlashStart;

    private Rigidbody2D rb;
    private GameObject currentCutTrail;
    private Vector2 touchPosition;
    private RaycastHit2D hit2;
    private bool cutAvailable=true;
    private bool ropeCut;
    private float startTime;
    private float performedTime;
    private float endTime;
    private bool soundOn = false;

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

    private void OnDestroy()
    {
        InputManager.playerInput.Player.Touch.started -= StartCutting;
        InputManager.playerInput.Player.TouchPosition.performed -= UpdateCut;
        InputManager.playerInput.Player.Touch.canceled -= StopCutting;

        LevelCanvas.onPause -= CutAvailable;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentCutTrail)
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
    }
    private void UpdateCut(InputAction.CallbackContext ctx)
    {
        if (cutAvailable)
        {
            if ((performedTime - startTime) > maxTime)
            {
                ropeCut = false;
                DestroyCutTrail();
            }

            touchPosition = ctx.ReadValue<Vector2>();

            rb.position = Camera.main.ScreenToWorldPoint(touchPosition);

            hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPosition), Vector2.zero);
            performedTime = (float)ctx.time;

            
            if (currentCutTrail && soundOn)
            {
                onSlash?.Invoke();
                soundOn = false;
            }
        }
    }
    private void StartCutting(InputAction.CallbackContext ctx)
    {
        if (cutAvailable)
        {
            soundOn = true;
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
            endTime = (float)ctx.time;
            DestroyCutTrail();

            if (ropeCut)
            {
                onCut?.Invoke();
                ropeCut = false;
            }
            soundOn = true;
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
