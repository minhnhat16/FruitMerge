using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SkinGrid : MonoBehaviour
{
    [SerializeField] private SkinItem crSkinItem;
    [SerializeField] private  List<SkinItem> _skins;
    [SerializeField] private int sumAvailableSkin ;
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

        var config = ConfigFileManager.Instance.ItemConfig.GetAllRecord();
        var playerData = DataAPIController.instance.GetAllFruitSkinOwned();
        for (int i = 0; i < config.Count; i++)
        {
            if (config[i].Type == ItemType.FRUITSKIN)
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
                    if (currentSkin == config[i].ID)
                    {
                        Debug.Log("CURRENT SKIN TRUEE" + currentSkin);
                        skin.GetComponent<SkinItem>().InitSkin(config[i].ID, true, false);
                        crSkinItem = skin.GetComponent<SkinItem>();
                    }
                    else if (playerData.Contains(config[i].ID))
                    {
                        Debug.Log(" CONTAIN SKIN " + config[i].ID);
                        skin.GetComponent<SkinItem>().InitSkin(config[i].ID, true, true);
                    }
                    else
                    {
                        Debug.Log("NOT CONTAIN SKIN");
                        skin.GetComponent<SkinItem>().InitSkin(config[i].ID, false, false);
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
    public void TabSetUp()
    {
        configs = ConfigFileManager.Instance.ItemConfig.GetAllRecord();

        if (_skins.Count == 0)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                var skinItem = Instantiate((Resources.Load("Prefab/UIPrefab/SkinItemPrefab", typeof(GameObject))), transform) as GameObject;
                ItemConfigRecord tabRecord = configs[i];
                _skins.Add(skinItem.GetComponent<SkinItem>());
                itemData.onItemSelect = () =>
                {
                    //OnItemSelect(tabRecord);
                };
                //skinItem.SetUp(tabRecord.ItemId, itemData);
            }
        }

        //UpdateTab();
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
