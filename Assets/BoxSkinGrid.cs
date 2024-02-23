using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoxSkinGrid : MonoBehaviour
{
    [SerializeField] private BoxItem crBoxItem;
    [SerializeField] private List<BoxItem> _skins;
    [SerializeField] private int sumAvailableSkin;
    [SerializeField] private static int ShopSkinId = 3;
    [SerializeField] private ScrollRect scrollRect;
    public UnityEvent<BoxItem> onEquipBoxAction = new UnityEvent<BoxItem>();
    [SerializeField] private FloatingText  floatingText;

    private void Awake()
    {

    }
    private void Start()
    {
        sumAvailableSkin = 6;
        InitiateSkinItem();
    }
    private void OnEnable()
    {
        ResetScroll();
        onEquipBoxAction.AddListener(SwitchCurrentSkin);
    }
    private void OnDisable()
    {
        onEquipBoxAction.RemoveListener(SwitchCurrentSkin);
    }
    private void InitiateSkinItem()
    {

        var itemConfig = ConfigFileManager.Instance.ItemConfig  .GetAllRecord();
        var priceConfig = ConfigFileManager.Instance.PriceConfig.GetAllRecord();
        var shopConfig = ConfigFileManager.Instance.ShopConfig.GetRecordByKeySearch(ShopSkinId);
        var playerData = DataAPIController.instance.GetAllFruitSkinOwned();
        for (int i = 0; i <itemConfig.Count; i++)
        {
            if (itemConfig[i].Type == ItemType.BOXSKIN)
            {
                var skin = Instantiate((Resources.Load("Prefab/UIPrefab/BoxSkin", typeof(GameObject))), transform) as GameObject;
                if (skin == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    skin.GetComponent<BoxItem>().onEquipActionBox = onEquipBoxAction;
                    int currentBoxSkin = DataAPIController.instance.GetCurrentBoxSkin();
                    _skins.Add(skin.GetComponent<BoxItem>());
                    if (currentBoxSkin == itemConfig[i].ID)
                    {
                        Debug.Log("CURRENT BOX SKIN TRUEE" + currentBoxSkin);
                        skin.GetComponent<BoxItem>().InitSkin(itemConfig[i].ID, true, false);
                        crBoxItem = skin.GetComponent<BoxItem>();
                    }
                    else if (playerData.Contains(itemConfig[i].ID))
                    {
                        Debug.Log(" CONTAIN BOX SKIN " + itemConfig[i].ID);
                        skin.GetComponent<BoxItem>().InitSkin(itemConfig[i].ID, true, true);
                    }
                    else
                    {
                        int idSkinInShop = shopConfig.IdPrice.Find(idprice => idprice == itemConfig[i].ID);
                        int price = priceConfig.Find(x => x.Id == idSkinInShop).Price;
                        skin.GetComponent<BoxItem>().Price = price;
                        skin.GetComponent<BoxItem>().InitSkin(itemConfig[i].ID, false, false);
                        Debug.Log("NOT CONTAIN SKIN");
                        Debug.Log("SKIN ID IN SHOP" + shopConfig.Id);
                        Debug.Log("itemConfig[i].Type = " + itemConfig[i].Type + " itemConfig[i].ID = " + itemConfig[i].ID + " shopSkinId = " + idSkinInShop);
                        Debug.Log("ITEM PRICE " + price);
                    }  
                }
            }

        }
    }
    public void SwitchCurrentSkin(BoxItem boxEquip)
    {
        Debug.Log("SWICTH CURRENT SKIN " + boxEquip.SkinID);
        crBoxItem.SetItemUnquiped();
        crBoxItem = boxEquip;
        floatingText.gameObject.SetActive(true);
        floatingText.ShowFloatingText();
    }
    public void ResetScroll()
    {
        if (scrollRect != null)
        {
            // Reset the scroll position to the top
            scrollRect.normalizedPosition = new Vector2(0f, 1f);
        }
        else
        {
            Debug.LogError("ScrollRect not assigned to the script.");
            return;
        }
    }
}
