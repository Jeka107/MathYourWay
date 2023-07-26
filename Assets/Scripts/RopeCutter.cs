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
    [SerializeField] private EdgeCollider2D myCollider;

    [SerializeField] private float maxTime;
    [SerializeField] private float minTimeSlashStart;

    [Space]
    [SerializeField] private Settings settings;

    private Rigidbody2D rb;
    private GameObject currentCutTrail;
    private Vector2 touchPosition;
    private RaycastHit2D hit2;
    private bool cutAvailable=true;
    private bool ropeCut;
    private float startTime;
    private float performedTime;
    private float endTime;
    private bool soundIsOn = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        InputManager.playerInput.Player.Touch.started += StartCutting;
        InputManager.playerInput.Player.TouchPosition.performed += UpdateCut;
        InputManager.playerInput.Player.Touch.canceled += StopCutting;

        TrailCollision.onPieceCut += PieceCut;
        LevelCanvas.onPause += CutAvailable;
        FirstLevelCanvas.onPause += CutAvailable;
        Settings.onPause += CutAvailable;
    }

    private void OnDestroy()
    {
        InputManager.playerInput.Player.Touch.started -= StartCutting;
        InputManager.playerInput.Player.TouchPosition.performed -= UpdateCut;
        InputManager.playerInput.Player.Touch.canceled -= StopCutting;

        TrailCollision.onPieceCut -= PieceCut;
        LevelCanvas.onPause -= CutAvailable;
        FirstLevelCanvas.onPause -= CutAvailable;
        Settings.onPause -= CutAvailable;
    }
    private void Update()
    {
        if (currentCutTrail)
        {
            SetColliderPointsFromTrail(currentCutTrail.GetComponent<TrailRenderer>(), myCollider);
        }
    }
    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();

        for (int pos = 0; pos < trail.positionCount; pos++)
        {
            points.Add(trail.GetPosition(pos));
        }
        collider.SetPoints(points);
    }

    private void PieceCut(GameObject collision)
    {
        if (currentCutTrail)
        {
            Transform hook = collision.transform.parent;

            Destroy(collision.gameObject);

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
            if ((performedTime - startTime) > maxTime)
            {
                ropeCut = false;
                DestroyCutTrail();
            }

            touchPosition = ctx.ReadValue<Vector2>();

            rb.position = Camera.main.ScreenToWorldPoint(touchPosition);

            performedTime = (float)ctx.time;

            if (currentCutTrail&&soundIsOn)
            {
                if (settings.GetSoundEffectStatus())
                {
                    onSlash?.Invoke();
                    soundIsOn = false;
                }
            }
        }
    }
    private void StartCutting(InputAction.CallbackContext ctx)
    {
        if (cutAvailable)
        {
            soundIsOn = true;
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
            soundIsOn = false;
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
