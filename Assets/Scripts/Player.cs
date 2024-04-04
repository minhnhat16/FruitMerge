using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private float rotation;

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
        dropCoolDown = 0.25f;
        StartCanDrop();
        WallScript.Instance.SetUpLineRender();
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        SetPlayerPosition();
        MouseDown();
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

            float x = Mathf.Clamp(spawnPoint.x, CameraMain.instance.GetLeft() + 0.5f, CameraMain.instance.GetRight() - 0.5f);
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
                        HandleTouchMoved(touch);
                        break;
                    case TouchPhase.Stationary:
                        // Handle touch stationary
                        HandleTouchStationary(touch);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        // Handle touch ended or canceled
                        HandleTouchEndedOrCanceled(touch);
                        break;
                }
            }
        }
    }
    private void HandleTouchBegan(Touch touch)
    {
        Debug.Log("Touch began at position: " + touch.position);
    }

    private void HandleTouchMoved(Touch touch)
    {
        Debug.Log("Touch moved at position: " + touch.position);
    }

    private void HandleTouchStationary(Touch touch)
    {
        Debug.Log("Touch stationary at position: " + touch.position);
    }

    private void HandleTouchEndedOrCanceled(Touch touch)
    {
        Debug.Log("Touch ended or canceled at position: " + touch.position);
    }
    void MouseDown()
    {
        if (CameraMain.instance != null && !IngameController.instance.isPause && canDrop == true && MousePosition() == true)
        {
            mainCircle = EndlessLevel.Instance.main;
            if (mainCircle != null) StartCoroutine(DropCircle());
        }
    }
    IEnumerator DropCircle()
    {
        yield return new WaitUntil(() => mainCircle != null);
        if (Input.GetMouseButtonDown(0))
        {
            mainCircle.transform.position = pos + circleSpawnPos;
            mainCircle.SetIsMerge(false);
            mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && canDrop && mainCircle != null)
        {
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
            mainCircle.transform.position = new Vector3(transform.position.x, CircleSpawnPos.y);

        }
        else if (Input.GetMouseButtonUp(0) && canDrop && mainCircle != null)
        {
            canDrop = false;
            DoGrapplingHook();
            _lineRenderer.gameObject.SetActive(false);
            //Debug.Log("Release mouse button");
            mainCircle.GotoState(mainCircle.Drop);
            EndlessLevel.Instance.intQueue.Remove(EndlessLevel.Instance.intQueue[0]);
            EndlessLevel.Instance.main = null;
            EndlessLevel.Instance.RandomCircle();
            IngameController.instance.FirstCircle();
            yield return new WaitForSeconds(dropCoolDown);
            _lineRenderer.gameObject.SetActive(true);
            canDrop = true;
        }
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
            left.OnComplete(()=>left.Kill(true));
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
}