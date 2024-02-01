using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SkinGrid : MonoBehaviour
{
    [SerializeField] private  List<SkinItem> _skins;
    [SerializeField] private int sumAvailableSkin ;
    [SerializeField] private ScrollRect scrollRect;
    private List<ItemConfigRecord> configs = new List<ItemConfigRecord>();
    private SkinViewItemAction itemData = new SkinViewItemAction();

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
    }
    private void InitiateSkinItem()
    {

        var config = ConfigFileManager.Instance.ItemConfig.GetAllRecord();

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
                    _skins.Add(skin.GetComponent<SkinItem>());
                    skin.GetComponent<SkinItem>().InitSkin((int)config[i].Type, true, false);
                }
            }
           
        }
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
        }
    }
    private void UpdateTab()
    {
        List<int> dataSkinList = DataAPIController.instance.GetAllFruitSkinOwned();

        foreach (SkinItem item in _skins)
        {
            item.transform.SetParent(transform.parent, false);

            if (dataSkinList.Contains(item.SkinID))
            {
                item.SetOwnedImg();
            }
        }
    }

    private void CheckObtainData(int idSkin)
    {
        var skinData = DataAPIController.instance.GetAllFruitSkinOwned();
    }
}
