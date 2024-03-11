using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeVFXPool : MonoBehaviour
{
    public BY_Local_Pool<MergeVFX> pool;
    public MergeVFX prefab;
    public static MergeVFXPool instance;
    [SerializeField] private int amount;
    private void Awake()
    {
        instance = this;
        pool = new BY_Local_Pool<MergeVFX>(prefab, amount, this.transform);
    }
}
