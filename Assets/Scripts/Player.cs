using NaughtyAttributes.Test;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour ,   IPointerDownHandler
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
    private void Awake()
    {
        instance = this;
        _render = GetComponentInChildren<SpriteRenderer>();
         transform.position = pos ;
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
        MouseDown();
        SetPlayerPosition();
    }
    public bool MousePosition()
    {
        var point = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log($"camera top{CameraMain.instance.GetTop() - 2f} " +
        //    $" point {point.x}");
        if (point.y > CameraMain.instance.GetTop() - 2f || point.y < CameraMain.instance.GetBottom() + 2f )
        {
            return false;
        }

        else return true;
    }
    private void SetPlayerPosition()
    {
        float x = Mathf.Clamp(spawnPoint.x, CameraMain.instance.GetLeft() + 1, CameraMain.instance.GetRight() - 1);
        transform.position = new Vector3(x, pos.y);
        _lineRenderer.SetPosition(0, transform.position);
        var linePos = _lineRenderer.GetPosition(1);
        _lineRenderer.SetPosition(1, new Vector3(transform.position.x, linePos.y));
    }
    void MouseDown()
    {

        if (gameObject.activeSelf ==true &&CameraMain.instance != null && MousePosition() && !IngameController.instance.isPause)
        {
            StartCoroutine(DropCircle());
            mainCircle = EndlessLevel.Instance.main;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        MouseDown();
    }
    IEnumerator DropCircle()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && canDrop && mainCircle != null)
        {

            spawnPoint = CameraMain.instance.main.ScreenToWorldPoint(Input.mousePosition);
            mainCircle.transform.position = new Vector3(gameObject.transform.position.x, 7.75f);
          
        }
        else if (Input.GetMouseButtonUp(0) && canDrop && mainCircle != null )
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
