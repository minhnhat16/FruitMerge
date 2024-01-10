using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BY_Local_Pool<T> where T : MonoBehaviour
{
    public T prefab;
    public Transform parent;
    public int total;
    [NonSerialized]
    public List<T> list = new List<T>();
    private int index = -1;
    public BY_Local_Pool(T prefab, int total, Transform parent = null)
    {
        this.parent = parent;
        this.prefab = prefab;
        this.total = total;
        index = -1;
        for (int i = 0; i < total; i++)
        {
            T trans = GameObject.Instantiate<T>(prefab);
            trans.transform.SetParent(parent);
            trans.gameObject.SetActive(false);
            list.Add(trans);

        }
    }
    public T SpawnNonGravity()
    {
        //Debug.Log("SPAWN");
        index++;
        if (index >= list.Count) index = 0;
        T trans = list[index];
        trans.gameObject.SetActive(true);
        return trans;

    }
    public T SpawnNonGravityWithIndex(int index)
    {
        // Debug.Log("spawn brick");
        this.index++;
        if (index >= list.Count) index = 0;
        T trans = list[index];
        trans.gameObject.SetActive(true);
        return trans;

    }
    public T SpawnGravity()
    {
        index++;
        if (index >= list.Count) index = 0;
        T trans = list[index];
        trans.gameObject.SetActive(true);
        trans.GetComponent<Rigidbody2D>().gravityScale = 1;
        return trans;
    }
    public void DeSpawnNonGravity(T trans)
    {
        trans.gameObject.SetActive(false);
    }
    public void DeSpawnGravity(T trans)
    {
        trans.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        trans.GetComponent<Rigidbody2D>().gravityScale = 0;
        trans.gameObject.SetActive(false);
    }
    public void DeSpawnAll()
    {
        foreach (var g in list)
        {
            if(g != null)
            {
                g.gameObject.SetActive(false);
            }
        }
        index = -1;
    }
}

