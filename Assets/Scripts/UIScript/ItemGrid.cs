using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemGrid : MonoBehaviour
{
    [SerializeField] private int total;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private List<Item> _items;
    [SerializeField] private ConfigField _config;

    private void Start()
    {
        InstantiateItems(); 
    }

    private void InstantiateItems()
    {
        for (int i  = 0; i < total; i++)
        {
            var item = Instantiate(_itemPrefab, transform);
            SetupItem(item);
        }   
    }

    private void SetupItem(GameObject item)
    {
        _items.Add(item.GetComponent<Item>());

    }
}
