using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackGroundInGame : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    public float scaleMultiplier ;
    public Vector2 imgScale;
    public Vector2 size ;

    void OnEnable()
    {
        ScaleBackground();
    }

    void ScaleBackground()
    {
        float scale = GameManager.instance.UIRoot.scale;
       transform.localScale += new Vector3(scale, scale, scale);
        //backgroundRenderer.size *= new Vector2(x , y);

    }
}
