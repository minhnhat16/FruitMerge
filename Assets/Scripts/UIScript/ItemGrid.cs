using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemGrid : MonoBehaviour
{
    [SerializeField] private static int idShop = 1;
    [SerializeField] private int total;
    [SerializeField] private ShopItemTemplate _itemPrefab;
    [SerializeField] private List<ShopItemTemplate> _items;


    private void Start()
    {
        InstantiateItems();
    }

    private void InstantiateItems()
    {
       var priceConfig = ConfigFileManager.Instance.PriceConfig.GetAllRecord();

        foreach (var i in priceConfig)
        {
            var priceRecord = ConfigFileManager.Instance.PriceConfig.GetRecordByKeySearch(i);
            if (priceRecord.IdShop == idShop)
            {
                var item = Instantiate(_itemPrefab, transform);
                SetupItem(item, priceRecord);
            };
           
        }
    }

    private void SetupItem(ShopItemTemplate item, PriceConfigRecord price)
    {
        var itemConfig = ConfigFileManager.Instance.ItemConfig.GetRecordByKeySearch(price.IdItem);
        item.Cost_lb.text= price.Price.ToString();
        item.ItemImg.sprite = SpriteLibControl.Instance.GetSpriteByName(itemConfig.SpriteName);
        item.Name_lb.text = itemConfig.Type.ToString();
        item.Total_lb.text = price.Amount.ToString();
        item.enabled = price.Available;
        _items.Add(item);

    }
}
