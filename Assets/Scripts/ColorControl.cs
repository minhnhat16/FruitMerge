using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ColorControl : MonoBehaviour
{
    public static ColorControl instance;

    private void Awake()
    {
        instance = this;    
    }
    public Color ColorByValue(int value)
    {
        return value switch
        {
            2 => SetColorFromHex("#D458BA"),
            4 => SetColorFromHex("#66D143"),
            8 => SetColorFromHex("#44CAC9"),
            16 => SetColorFromHex("#3C84D6"),
            32 => SetColorFromHex("#DF614A"),
            64 => SetColorFromHex("#8C7CFF"),
            128 => SetColorFromHex("#948A81"),
            256 => SetColorFromHex("#FFAB36"),
            512 => SetColorFromHex("#FD556F"),
            1024 => SetColorFromHex("#FD9155"),
            2048 => SetColorFromHex("#55FD8E"),
            4096 => SetColorFromHex("#FF4A4A"),
            _ => SetColorFromHex("#000000"),
        };
    }
    Color SetColorFromHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogError($"Failed to parse color from hex code: {hex}");
            return Color.black;
        }
    }
}
