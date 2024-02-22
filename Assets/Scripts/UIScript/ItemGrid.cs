using System.Collections.Generic;
using UnityEngine;

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
        var shopConfig = ConfigFileManager.Instance.ShopConfig.GetRecordByKeySearch(idShop);
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
                var priceConfig = ConfigFileManager.Instance.PriceConfig.GetRecordByKeySearch(i);
                SetupItem(item.GetComponent<ShopItemTemplate>(), priceConfig);
            }
        }
    }
    private void SetupItem(ShopItemTemplate item, PriceConfigRecord price)
    {
        if (price == null)
        {
            Debug.Log("Null config");
            return;
        }
        var itemConfig = ConfigFileManager.Instance.ItemConfig.GetRecordByKeySearch(price.IdItem);
        item.IntCost = price.Price;
        item.ItemImg.sprite = SpriteLibControl.Instance.GetSpriteByName(itemConfig.SpriteName);
        item.Type = price.IdItem;
        item.TotalItem =  price.Amount;
        item.Enable = price.Available;
        //Debug.Log(price.Available.ToString() + " and item: "+ item.enabled.ToString());
        _items.Add(item);
    }
}
