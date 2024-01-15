using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public Vector3 Pos { get { return pos; } }
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
        MouseDown();
        SetPlayerPosition();
    }
    public bool MousePosition()
    {
        var point = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log($"camera top{CameraMain.instance.GetTop() - 2f} " +
        //    $" point {point.x}");
        if (point.y > CameraMain.instance.GetTop() - 2f || point.y < CameraMain.instance.GetBottom() + 2f)
        {
            return false;
        }

        else return true;
    }
    private void SetPlayerPosition()
    {
        float x = Mathf.Clamp(spawnPoint.x, CameraMain.instance.GetLeft() + 1, CameraMain.instance.GetRight() - 1);
        transform.position = new Vector3(x, pos.y);
        _lineRenderer.SetPosition(0, transform.position - new Vector3(0,0.15f));
        var linePos = _lineRenderer.GetPosition(1);
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x, linePos.y));
    }
    void MouseDown()
    {

        if (CameraMain.instance != null && MousePosition() && !IngameController.instance.isPause)
        {
            mainCircle = EndlessLevel.Instance.main;
            if (mainCircle != null) StartCoroutine(DropCircle());
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
        }
    }
    IEnumerator DropCircle()
    {
        yield return new WaitUntil(() => mainCircle != null);
        if (Input.GetMouseButtonDown(0))
        {
            mainCircle.SetIsMerge(false);
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && canDrop && mainCircle != null)
        {

            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
            mainCircle.transform.position = new Vector3(gameObject.transform.position.x, pos.y);

        }
        else if (Input.GetMouseButtonUp(0) && canDrop && mainCircle != null)
        {
            canDrop = false;
            //Debug.Log("Release mouse button");
            mainCircle.GotoState(mainCircle.Drop);
            EndlessLevel.Instance.intQueue.Remove(EndlessLevel.Instance.intQueue[0]);
            EndlessLevel.Instance.main = null;
            EndlessLevel.Instance.RandomCircle();
            IngameController.instance.FirstCircle();
            yield return new WaitForSeconds(dropCoolDown);
            canDrop = true;
        }
    }
}