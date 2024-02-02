using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LineScript : BackGroundInGame
{
    private void OnEnable()
    {
        Debug.Log(transform.position);
        transform.position = Player.instance.Pos;
    }
}
