using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static WallScript Instance;
    [SerializeField] private List<Collider2D> colliders;
    [SerializeField] private SpriteRenderer _box;
    public Vector3 shakeAmount = new Vector3(5f,5f, 0f); // Adjust the shake amount along each axis
    public float shakeDuration = 0.5f;
    public int vibrato =30;
    public float randomness = 90f;
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
            _box.size *= GameManager.instance.UIRoot.rate;
            colliders[0].transform.position = new Vector2(CameraMain.instance.GetLeft() - 0.5f, 0f);
            colliders[1].transform.position = new Vector2(CameraMain.instance.GetRight() + 0.5f, 0f);
            colliders[3].transform.position = new Vector2(0f, CameraMain.instance.GetBottom() + 3);
            colliders[2].transform.position = colliders[3].transform.position + new Vector3(0, 11f, 0);
        }

    }
    public void TopWallCouroutine()
    {
        StartCoroutine(SetTopWallActive());
    }
    private IEnumerator SetTopWallActive()
    {
        yield return new WaitForSeconds(timeSetTopWall);
        colliders[2].gameObject.SetActive(true);
        Debug.Log($"SetTopWallActive {colliders[2].isActiveAndEnabled}");
    }
    public void SetUpLineRender()
    {
        var line = Player.instance.LineRenderer;
        Vector3 vector3 = colliders[3].transform.position + new Vector3(0, 0.5f);
        Player.instance.DropLinePos = vector3;
        line.SetPosition(1, vector3);
        Player.instance.SetLineRenderer(line);
    }
    public void ShakeWall()
    {
        // Shake the object along the X axis
        var tween = transform.DOShakePosition(shakeDuration, shakeAmount, vibrato, randomness, false, true, ShakeRandomnessMode.Full)
        .SetLoops(10)
            .SetRelative(true); // Loop time  = 10sec
        tween.OnComplete(() =>
        {
            Player.instance.canDrop = true;
            transform.position = Vector3.zero;
            IngameController.instance.CancelItem();
            tween?.Kill();
        });
    }


}
