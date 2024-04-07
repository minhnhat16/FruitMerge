using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player instance;
    public bool canDrop = false;
    public Vector2 spawnPoint;
    public CircleObject mainCircle;
    [SerializeField] private float dropCoolDown;
    [SerializeField] private List<SpriteRenderer> _renders;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector3 dropLinePos;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 circleSpawnPos;
    [SerializeField] private float leftCamPos;
    [SerializeField] private float rightCamPos;
    [SerializeField] private float rotation;

    [HideInInspector]
    public UnityEvent<bool> onTouchStart = new();
    [HideInInspector]
    public UnityEvent<bool> onTouchHold = new();
    [HideInInspector]
    public UnityEvent<bool> onTouchRelease = new();
    [HideInInspector]
    public UnityEvent<bool> onCircleDropped = new();
    public Vector3 Pos { get { return pos; } }
    public Vector3 CircleSpawnPos { get { return circleSpawnPos; } }

    public LineRenderer LineRenderer { get { return _lineRenderer; } }
    public Vector3 DropLinePos { get { return dropLinePos; } set { dropLinePos = value; } }
    public void SetLineRenderer(LineRenderer line)
    {
        this._lineRenderer = line;
    }
    private void Awake()
    {
        instance = this;
        transform.position = pos;
        circleSpawnPos += pos;
    }
    private void OnEnable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        Input.multiTouchEnabled = false;
        dropCoolDown = 0.25f;
        StartCanDrop();
        WallScript.Instance.SetUpLineRender();
        if (CameraMain.instance != null)
        {
            leftCamPos = CameraMain.instance.GetLeft();
            rightCamPos = CameraMain.instance.GetRight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        TouchHandle();
    }
    public void ResetPos()
    {
        spawnPoint = new Vector3(0, spawnPoint.y);
        transform.position = new Vector3(0, transform.position.y);
    }

    public void StartCanDrop()
    {
        StartCoroutine(CanDropPlayer());
    }
    IEnumerator CanDropPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        canDrop = true;
    }
    public bool MousePosition()
    {
        var point = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        //bool isPointerOverUI = GameManager.instance.UIRoot.IsPointerOverUIElement();
        if (point.y > CameraMain.instance.GetTop() - 3.5f || point.y < CameraMain.instance.GetBottom() + 4.5f)
        {
            return false;
        }
        else return true;
    }
    private void SetPlayerPosition()
    {
        if (CameraMain.instance.main != null)
        {
            float x = Mathf.Clamp(spawnPoint.x, leftCamPos + 0.5f, rightCamPos - 0.5f);
            transform.position = new Vector3(x, pos.y);
            _lineRenderer.SetPosition(0, transform.position + new Vector3(0, 0.35f));
            var linePos = _lineRenderer.GetPosition(1);
            _lineRenderer.SetPosition(1, new Vector3(transform.position.x, linePos.y));
        }
    }
    void TouchHandle()
    {
        if (Input.touchCount > 0)
        {
            // Iterate through each touch
            SetPlayerPosition();

            foreach (Touch touch in Input.touches)
            {
                // Check the phase of the touch
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // Handle touch began
                        HandleTouchBegan(touch);
                        break;
                    case TouchPhase.Moved:
                        // Handle touch moved
                        if (mainCircle == null) break;
                        HandleTouchMoved(touch);
                        break;
                    case TouchPhase.Stationary:
                        if (mainCircle == null) break;
                        // Handle touch stationary
                        HandleTouchStationary(touch);
                        break;
                    case TouchPhase.Ended:

                    case TouchPhase.Canceled:
                        // Handle touch ended or canceled
                        HandleTouchEndedOrCanceled(touch);
                        break;
                    default : break;
                }
            }
        }
    }
    private void HandleTouchBegan(Touch touch)
    {
        mainCircle = EndlessLevel.Instance.main;
        if (mainCircle == null) return;
        pos.x = touch.position.x;
        transform.position = pos;
        //Debug.Log("Touch began at position: " + touch.position);
        //Debug.Log($" mainC {mainCircle.transform.position} = pos{pos}+ circleSpawnPos{circleSpawnPos};");
        mainCircle.transform.position = pos + circleSpawnPos;
        mainCircle.SetIsMerge(false);
        mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);
        spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(touch.position);
    }

    private void HandleTouchMoved(Touch touch)
    {
        //Debug.Log("Touch moved at position: " + touch.position);
        spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(touch.position);
        mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);
    }

    private void HandleTouchStationary(Touch touch)
    {
        spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(touch.position);
        mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);
        //Debug.Log("Touch stationary at position: " + touch.position);
    }

    private void HandleTouchEndedOrCanceled(Touch touch)
    {
        //Debug.Log("Touch ended or canceled at position: " + touch.position);
        DoDropCircle();
    }
    private void DoDropCircle()
    {
        if (mainCircle == null) return;
        canDrop = false;
        DoGrapplingHook();
        _lineRenderer.gameObject.SetActive(false);
        mainCircle.GotoState(mainCircle.Drop);
        mainCircle = null;
        onCircleDropped?.Invoke(true);
        _lineRenderer.gameObject.SetActive(true);
    }
    void MouseDown()
    {
        mainCircle = EndlessLevel.Instance.main;
        if (CameraMain.instance != null && !IngameController.instance.isPause && canDrop == true && MousePosition() == true)
        {

            if (mainCircle != null)
            {
                DropCircle();
            }
        }
    }
    IEnumerator DropCircle()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(0))
        {
            mainCircle.transform.position = pos + circleSpawnPos;
            mainCircle.SetIsMerge(false);
            mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && canDrop && mainCircle != null)
        {
            circleSpawnPos =
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
            mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);

        }
        else if (Input.GetMouseButtonUp(0) /*&& canDrop && mainCircle != null*/)
        {
            DoGrapplingHook();
            canDrop = false;
            _lineRenderer.gameObject.SetActive(false);
            mainCircle.GotoState(mainCircle.Drop);

            onCircleDropped?.Invoke(true);
            yield return new WaitForSeconds(0.5f);
            canDrop = true;
            _lineRenderer.gameObject.SetActive(true);
        }
    }
    IEnumerator ReleaseToDrop()
    {
        yield return new WaitForSeconds(0.5f);

    }
    Tween left;
    Tween right;
    public void DoGrapplingHook()
    {
        Vector3 rotateAngle = new Vector3(0, 0, rotation);
        left = _renders[0].transform.DORotate(-rotateAngle, 0.2f, RotateMode.Fast);
        right = _renders[1].transform.DORotate(rotateAngle, 0.2f, RotateMode.Fast);
        left.OnComplete(() =>
        {
            left = _renders[0].transform.DORotate(Vector3.zero, 0.25f, RotateMode.Fast);
            left.OnComplete(() => left.Kill(true));
        });
        right.OnComplete(() =>
        {
            right = _renders[1].transform.DORotate(Vector3.zero, 0.25f, RotateMode.Fast);
            right.OnComplete(() => right.Kill(true));
        });

    }
    public void Reset()
    {
        ResetPos();
        canDrop = true;
        mainCircle = null;
    }
    public void SetUpLineRender()
    {
        //Vector3 vector3 = colliders[3].transform.position + new Vector3(0, 0.5f);
        dropLinePos = WallScript.Instance.GetBoxPosition();
        dropLinePos.x = pos.x;
        _lineRenderer.SetPosition(1, dropLinePos);
    }
}