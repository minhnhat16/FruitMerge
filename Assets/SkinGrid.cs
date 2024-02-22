using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinGrid : MonoBehaviour
{
    [SerializeField] private SkinItem crSkinItem;
    [SerializeField] private List<SkinItem> _skins;
    [SerializeField] private int sumAvailableSkin;
    [SerializeField] private static int ShopSkinId = 2;
    [SerializeField] private ScrollRect scrollRect;
    private List<ItemConfigRecord> configs = new List<ItemConfigRecord>();
    private SkinViewItemAction itemData = new SkinViewItemAction();
    public UnityEvent<SkinItem> onEquipAction = new UnityEvent<SkinItem>();

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
        onEquipAction.AddListener(SwitchCurrentSkin);
    }
    private void OnDisable()
    {
        onEquipAction.RemoveListener(SwitchCurrentSkin);
    }
    private void InitiateSkinItem()
    {

        var itemConfig = ConfigFileManager.Instance.ItemConfig.GetAllRecord();
        var priceConfig = ConfigFileManager.Instance.PriceConfig.GetAllRecord();
        var shopConfig = ConfigFileManager.Instance.ShopConfig.GetRecordByKeySearch(ShopSkinId);
        var playerData = DataAPIController.instance.GetAllFruitSkinOwned();
        for (int i = 0; i < itemConfig.Count; i++)
        {
            if (itemConfig[i].Type == ItemType.FRUITSKIN)
            {
                var skin = Instantiate((Resources.Load("Prefab/UIPrefab/SkinItemPrefab", typeof(GameObject))), transform) as GameObject;
                if (skin == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    skin.GetComponent<SkinItem>().onEquipAction = onEquipAction;
                    int currentSkin = DataAPIController.instance.GetCurrentFruitSkin();
                    _skins.Add(skin.GetComponent<SkinItem>());
                    if (currentSkin == itemConfig[i].ID)
                    {
                        Debug.Log("CURRENT SKIN TRUEE" + currentSkin);
                        skin.GetComponent<SkinItem>().InitSkin(itemConfig[i].ID, true, false);
                        crSkinItem = skin.GetComponent<SkinItem>();
                    }
                    else if (playerData.Contains(itemConfig[i].ID))
                    {
                        Debug.Log(" CONTAIN SKIN " + itemConfig[i].ID);
                        skin.GetComponent<SkinItem>().InitSkin(itemConfig[i].ID, true, true);
                    }
                    else
                    {
                        int idSkinInShop = shopConfig.IdPrice.Find(idprice => idprice == itemConfig[i].ID);
                        int price = priceConfig.Find(x => x.Id == idSkinInShop).Price;
                        skin.GetComponent<SkinItem>().Price = price;
                        skin.GetComponent<SkinItem>().InitSkin(itemConfig[i].ID, false, false);
                        
                      
                        //Debug.Log("NOT CONTAIN SKIN");
                        //Debug.Log("SKIN ID IN SHOP" + shopConfig.Id);
                        //Debug.Log("itemConfig[i].Type = " + itemConfig[i].Type + " itemConfig[i].ID = " + itemConfig[i].ID + " shopSkinId = " + idSkinInShop);
                        //Debug.Log("ITEM PRICE " + price);
                    }
                }
            }

        }
    }
    public void SwitchCurrentSkin(SkinItem skinEquip)
    {
        Debug.Log("SWICTH CURRENT SKIN " + skinEquip.SkinID);
        crSkinItem.SetItemUnquiped();
        crSkinItem = skinEquip;
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
