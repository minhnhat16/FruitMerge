using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static WallScript Instance;
    [SerializeField] private List<Collider2D> colliders;
    [SerializeField] private SpriteRenderer _box;
    public float timeSetTopWall = 10f;
    public GameObject GetTopWall()
    {
        return colliders[2].gameObject;
    }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetUpCollider();
    }

    // Update is called once per frame
    void Update()
    {
    }   
    void SetUpCollider()
    {
        if (CameraMain.instance != null)
        {
            ScaleByScreen();
        }

    }
    void ScaleByScreen()
    {
        float currentRate =  (float)Screen.height / (float)Screen.width;
        Debug.Log($" {(float)Screen.height} / {(float)Screen.width} rate {currentRate}");
        if (currentRate < 2) 
        {
            Debug.Log("ScaleByScreen not in 2960/1440");
            float mainRate = 1080f / 1920f;
            _box.transform.localScale += Vector3.one * mainRate;
            _box.transform.position = new Vector2(0f, CameraMain.instance.GetBottom() + 3.75f   + mainRate );
        }
        else
        {
            Debug.Log("ScaleByScreen in 2960/1440");
            float mainRate = 1080f / 1920f;
            _box.transform.localScale += Vector3.one * 0.35f;
            _box.transform.position = new Vector2(0f, CameraMain.instance.GetBottom() + 3.75f + mainRate * currentRate);
        }
    }
    public void TopWallCouroutine()
    {
        StartCoroutine(SetTopWallActive());
    }
    public void SetTopWallActive(bool active)
    {
        colliders[2].gameObject.SetActive(active);

    }
    public void SetBoxPosition()
    {
        Debug.Log($" CameraMain.instance.GetBottom({CameraMain.instance.GetBottom()})");
    }
    private IEnumerator SetTopWallActive()
    {
        yield return new WaitForSeconds(timeSetTopWall);
        SetTopWallActive(true);
        Debug.Log($"SetTopWallActive {colliders[2].isActiveAndEnabled}");
    }
    public void SetUpLineRender()
    {
        var line = Player.instance.LineRenderer;
        //Vector3 vector3 = colliders[3].transform.position + new Vector3(0, 0.5f);
        Vector3 vector3 = _box.transform.position;
        Player.instance.DropLinePos = vector3;
        line.SetPosition(1, vector3);
        Player.instance.SetLineRenderer(line);
    }
}
