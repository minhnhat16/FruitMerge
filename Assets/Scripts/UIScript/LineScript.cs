using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LineScript : BackGroundInGame
{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Vector3 fixPos;
    private void OnEnable()
    {
        //Debug.Log(transform.position);
        fixPos = Player.instance.transform.position - new Vector3(0,4.75f);
        transform.position = fixPos;
        SetSpriteRenderToCameraScale();
    }
    private void Update()
    {
        
    }
    public void SetSpriteRenderToCameraScale()
    {
        float x = CameraMain.instance.width; 
        render.size = new Vector2(x*2f,render.size.y);
    }
}
