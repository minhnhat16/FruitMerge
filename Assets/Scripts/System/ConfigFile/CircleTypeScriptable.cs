using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CircleTypeScriptable", menuName = "ScriptableObjects/CircleTypeScriptable ")]
public class CircleTypeScriptable : ScriptableObject
{
    public Sprite[] spriteType;
    public Vector3[] scale;
    public float[] radius;

}

