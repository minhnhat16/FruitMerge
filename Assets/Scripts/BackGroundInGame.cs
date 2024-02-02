using System.Collections;
using UnityEngine;

public class BackGroundInGame : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer;

    public float scaleMultiplier = 0.001f;

    void OnEnable()
    {
        ScaleBackground();
    }

    void ScaleBackground()
    {
        float screenWidth = Screen.width * scaleMultiplier;
        float screenHeight = Screen.height * scaleMultiplier;

        backgroundRenderer.size = new Vector2(screenWidth, screenHeight);

    }
}
