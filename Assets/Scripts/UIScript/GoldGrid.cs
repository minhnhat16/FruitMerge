using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
public class GoldGrid : MonoBehaviour
{
    [SerializeField] private static int idShop = 0;
    [SerializeField] private int total;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private List<Item> _golds;
    [SerializeField] private ConfigField _config;
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
            Debug.Log("Have price config");
            if (i.IdShop == idShop)
            {
                var item = Instantiate((Resources.Load("Prefab/UI/ShopItemTemplate", typeof(GameObject))), transform) as GameObject;
                if (item == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {

                    _items.Add(item.GetComponent<ShopItemTemplate>());
                    SetupItem(item.GetComponent<ShopItemTemplate>(), i);

                }
            };

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

        item.Cost_lb.text = price.Price.ToString();
        item.ItemImg.sprite = SpriteLibControl.Instance.GetSpriteByName(itemConfig.SpriteName);
        item.Name_lb.text = itemConfig.Type.ToString();
        item.Total_lb.text = "x" + price.Amount.ToString();
        item.enabled = price.Available;
        _items.Add(item);

    }
}
