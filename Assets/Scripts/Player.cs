using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPointerClickHandler
{
    public static Player instance;
    public bool canDrop = false;
    public Vector2 spawnPoint;
    public CircleObject mainCircle;
    [SerializeField] private float dropCoolDown;
    [SerializeField] private SpriteRenderer _render;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 circleSpawnPos;
    public Vector3 Pos { get { return pos; } }
    public Vector3 CircleSpawnPos { get { return circleSpawnPos; } }

    public LineRenderer LineRenderer { get { return _lineRenderer; } }
    public void SetLineRenderer(LineRenderer line)
    {
        this._lineRenderer = line;
    }
    private void Awake()
    {
        instance = this;
        _render = GetComponentInChildren<SpriteRenderer>();
        transform.position = pos;
        circleSpawnPos += pos;
    }
    // Start is called before the first frame update
    void Start()
    {
        dropCoolDown = 0.25f;
        canDrop = true;
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
    public bool MousePosition()
    {
        var point = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        //Debug.Log($"camera top{CameraMain.instance.GetTop() - 2f} " +
        //    $" point {point.x}");
        if (point.y > CameraMain.instance.GetTop() - 3f || point.y < CameraMain.instance.GetBottom() + 4f && !isPointerOverUI)
        {
            return false;
        }

        else return true;
    }
    private void SetPlayerPosition()
    {
        if (CameraMain.instance.main == null) return;
        float x = Mathf.Clamp(spawnPoint.x, CameraMain.instance.GetLeft() + 0.5f, CameraMain.instance.GetRight() - 0.5f);
        transform.position = new Vector3(x, pos.y);
        _lineRenderer.SetPosition(0, transform.position  + new Vector3(0, 0.35f));
        var linePos = _lineRenderer.GetPosition(1);
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x, linePos.y));
    }
    void MouseDown()
    {

        if (CameraMain.instance != null && MousePosition() && !IngameController.instance.isPause && canDrop ==true )
        {
            mainCircle = EndlessLevel.Instance.main;
            if (mainCircle != null) StartCoroutine(DropCircle());
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the pointer is over a UI GameObject
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Graphic>() != null)
        {
            canDrop = false;
            Debug.Log("On Pointer Click On GameObject");
        }
        else
        {
            canDrop = true;
            Debug.Log("On Pointer Not Click On GameObject");
        }
    }
    IEnumerator DropCircle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            yield return new WaitUntil(() => mainCircle != null);
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
}