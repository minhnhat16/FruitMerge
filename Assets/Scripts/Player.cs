using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public bool canDrop = false;
    public Vector2 spawnPoint;
    public CircleObject mainCircle;
    private float dropCoolDown;
    [SerializeField] private SpriteRenderer _render;
    [SerializeField] private LineRenderer _lineRenderer;
    public Vector3 Pos { get { return transform.position; } }
    private void Awake()
    {
        instance = this;
        _render = GetComponentInChildren<SpriteRenderer>();
        transform.position = new Vector3(0, 7.75f);
    }
    // Start is called before the first frame update
    void Start()
    {
        dropCoolDown = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.position = new Vector3(x, 7.75f);
        _lineRenderer.SetPosition(0, transform.position);
        var pos = _lineRenderer.GetPosition(1);
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x, pos.y));
    }
    void MouseDown()
    {

        if (CameraMain.instance != null && MousePosition() && !IngameController.instance.isPause)
        {
            StartCoroutine(DropCircle());
            mainCircle = EndlessLevel.Instance.main;
        }
    }
    IEnumerator DropCircle()
    {
        if (Input.GetMouseButtonDown(0) && !canDrop)
        {
            canDrop = true;
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && canDrop && mainCircle != null)
        {

            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
            mainCircle.transform.position = new Vector3(gameObject.transform.position.x, 7.75f);
            yield return new WaitForSeconds(dropCoolDown);

        }
        else if (Input.GetMouseButtonUp(0) && canDrop && mainCircle != null)
        {
            //Debug.Log("Release mouse button");
            mainCircle.GotoState(mainCircle.Drop);
            EndlessLevel.Instance.intQueue.Remove(EndlessLevel.Instance.intQueue[0]);
            EndlessLevel.Instance.main = null;
            EndlessLevel.Instance.RandomCircle();
            EndlessLevel.Instance.AddCircle(mainCircle);
            IngameController.instance.FirstCircle();
            canDrop = false;
        }
    }
}
