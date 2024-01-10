using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePool : MonoBehaviour
{
    public BY_Local_Pool<CircleObject> pool;
    public CircleObject prefab;
    public static CirclePool instance;
    private void Awake()
    {
        instance = this;
        pool = new BY_Local_Pool<CircleObject>(prefab, 500, this.transform);
    }
}
