using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Debug = UnityEngine.Debug;

public class ItemGrid : MonoBehaviour
{
    [SerializeField] private static int idShop = 1;
    [SerializeField] private int total;
    [SerializeField] private ShopItemTemplate _itemPrefab;
    [SerializeField] private List<ShopItemTemplate> _items;
    [SerializeField] ShopConfigRecord shopConfig = new ShopConfigRecord();
    [SerializeField] List<ItemConfigRecord> itemConfig = new();
    [SerializeField] List<PriceConfigRecord> priceConfigRecords = new();

    private void Start()
    {
        InstantiateItems();
        shopConfig = ConfigFileManager.Instance.ShopConfig.GetRecordByKeySearch(idShop);
        itemConfig = ConfigFileManager.Instance.ItemConfig.GetAllRecord();
        priceConfigRecords = ConfigFileManager.Instance.PriceConfig.GetAllRecord();
        InstantiateItems();

    }
    private void InstantiateItems()
    {
        bool isEnable;
        int id;
        int price;
        string spriteName;
        ItemType type;
        int total;
        foreach (var i in shopConfig.IdPrice)
        {
            Debug.Log("Have price config" + idShop);

            var item = Instantiate((Resources.Load("Prefab/UIPrefab/ShopItemTemplate", typeof(GameObject))), transform) as GameObject;
            if (item == null)
            {
                Debug.LogError(" item == null");
            }
            else
            {
                _items.Add(item.GetComponent<ShopItemTemplate>());
                var priceItem = priceConfigRecords[i];
                ShopItemTemplate newItem = new ShopItemTemplate();
                id = priceItem.IdItem;
                price = priceItem.Price;
                spriteName = itemConfig[id].SpriteName;
                type = itemConfig[id].Type;
                total = priceItem.Amount;
                isEnable = priceItem.Available;
                newItem.SetupItem(id, price, spriteName, type, total, isEnable);
                _items.Add(newItem);
            }
        }
    }
  
}
