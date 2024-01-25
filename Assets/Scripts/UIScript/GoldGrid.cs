using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class GoldGrid : MonoBehaviour
{
    [SerializeField] private int total;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private List<Item> _golds;
    [SerializeField] private ConfigField _config;

    private void Start()
    {
        InstantiateItems();
    }

    private void InstantiateItems()
    {
        for (int i = 0; i < total; i++)
        {
            var item = Instantiate(_itemPrefab, transform);
            SetupItem(item);
        }
    }

    private void SetupItem(GameObject item)
    {
        _golds.Add(item.GetComponent<Item>());

    }
}
