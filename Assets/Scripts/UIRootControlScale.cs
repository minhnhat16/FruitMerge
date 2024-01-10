using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRootControlScale : MonoBehaviour
{
    [SerializeField] CanvasScaler[] canvasScalers;
    public float rate;
    private void Start()
    {
        rate = 1080f / 1920f;
        //Debug.Log("WIDTH AND HIGHT" + Screen.width + " " + Screen.height);

        float currentRate = (float)Screen.width / (float)Screen.height;
        float scale = currentRate > rate ? 1 : 0;
        //Debug.Log("CURRENT RATE" + (currentRate > rate ? 1 : 0));
        foreach (CanvasScaler cs in canvasScalers)
        {
            //Debug.Log("CanvasScaler" + cs +  "Scale" + scale);
            cs.matchWidthOrHeight = scale;
        }
    }
}
