using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackGroundInGame : MonoBehaviour
{
    public Canvas canvas;
    public SpriteRenderer UpBG;
    public SpriteRenderer DownBG;


    private void Start()
    {
        canvas = GetComponent<Canvas>();
        if(canvas!= null)
        {
            Debug.Log("Canvas cam not null");
            canvas.worldCamera = CameraMain.instance.main;
        }
        else
        {
            Debug.Log("Canvas cam null");
        }
    }
}
