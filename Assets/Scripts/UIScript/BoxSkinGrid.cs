using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoxSkinGrid : MonoBehaviour
{
    [SerializeField] public static BoxSkinGrid Instance;
    [SerializeField] private BoxItem crBoxItem;
    [SerializeField] private List<BoxItem> _skins;
    [SerializeField] private static int ShopSkinId = 3;
    [SerializeField] private ScrollRect scrollRect;
    public UnityEvent<BoxItem> onEquipBoxAction = new UnityEvent<BoxItem>();
    [SerializeField] private FloatingText floatingText;

    [SerializeField] private List<ItemConfigRecord> itemConfig = new();
    [SerializeField] private List<PriceConfigRecord> priceConfig = new();
    [SerializeField] private ShopConfigRecord shopConfig = new();
    [SerializeField] private List<int> playerData = new();
    private void Awake()
    {
        if (Instance != null)
            Instance = this;
    }
    private void Start()
    {

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
    public void StartSetupItem()
    {
    }
    public void SetupBoxGrid()
    {
        Debug.Log("SetupItem BoxSkin ");
        Debug.Log("SetupItem BoxSkin when done");
        itemConfig = ConfigFileManager.Instance.ItemConfig.GetAllRecord();
        priceConfig = ConfigFileManager.Instance.PriceConfig.GetAllRecord();
        shopConfig = ConfigFileManager.Instance.ShopConfig.GetRecordByKeySearch(ShopSkinId);
        playerData = DataAPIController.instance.GetAllFruitSkinOwned();
        InitiateSkinItem();
    }
    private void InitiateSkinItem()
    {
        int currentBoxSkin = DataAPIController.instance.GetCurrentBoxSkin();
        for (int i = 0; i < itemConfig.Count; i++)
        {
            if (itemConfig[i].Type == ItemType.BOXSKIN)
            {
                var skin = Instantiate((Resources.Load("Prefab/UIPrefab/BoxSkin", typeof(GameObject))), scrollRect.content) as GameObject;
                if (skin == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    skin.GetComponent<BoxItem>().onEquipActionBox = onEquipBoxAction;
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
                        int price = priceConfig.Find(x => x.IdItem == idSkinInShop).Price;
                        skin.GetComponent<BoxItem>().Price = price;
                        skin.GetComponent<BoxItem>().InitSkin(itemConfig[i].ID, false, false);
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
