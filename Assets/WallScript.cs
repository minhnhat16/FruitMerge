using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Collider2D> colliders;
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
            
            colliders[0].transform.position = new Vector2(CameraMain.instance.GetLeft() - 0.5f, 0f);

            colliders[1].transform.position = new Vector2(CameraMain.instance.GetRight() + 0.5f, 0f);

            colliders[2].transform.position = new Vector2(0f, CameraMain.instance.GetTop() -2f);

            colliders[3].transform.position = new Vector2(0f, CameraMain.instance.GetBottom() + 1.25f);
        }

    }
 


}
