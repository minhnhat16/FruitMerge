
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DataAPIController : MonoBehaviour
{
    public static DataAPIController instance;

    [SerializeField]
    private DataModel dataModel;

    private void Awake()
    {
        instance = this;
    }

    public void InitData(Action callback)
    {
        Debug.Log("(BOOT) // INIT DATA");

        dataModel.InitData(() =>
        {
            // CheckDailyLogin();
            callback();
        });
        Debug.Log("==========> BOOT PROCESS SUCCESS <==========");
    }

    #region Get Data
    public void Level()
    {
        Debug.Log("DATA === LEVEL");

        dataModel.ReadData<string>(DataPath.NAME);
    }
 
    public int GetGold()
    {
        //Debug.LogWarning("GETTING GOLD .....");
        int gold = dataModel.ReadData<int>(DataPath.GOLD);
        //int gold = 0;
        return gold;
    }
    public void MinusGold(int minus)
    {
        int gold = dataModel.ReadData<int>(DataPath.GOLD);
        gold -= minus;
        SaveGold(gold,null);
    }
    public int GetHighestLevel()
    {
        //Debug.Log("DATA === highestLevel");
        int level = dataModel.ReadData<int>(DataPath.HIGHESTLV);
        return level;
    }

  
    public int GetCurrentLevel()
    {
        int level = dataModel.ReadData<int>(DataPath.CURRENTLV);
        return level;
    }
    #endregion

    public void SaveHighestLevel(int level)
    {
        int highlevel = GetHighestLevel();
        if (highlevel < level)
        {
            dataModel.UpdateData(DataPath.HIGHESTLV, level);
            dataModel.UpdateData(DataPath.CURRENTLV, level);
        }
    }
    public void SaveCurrentLevel(int level) {
        dataModel.UpdateData(DataPath.CURRENTLV, level);
    }

    #region Others
    public ItemData GetItemData(string type)
    {
        Debug.Log("DATA === ITEM DATA");
        ItemData itemData = dataModel.ReadDictionary<ItemData>(DataPath.ITEM, type.ToString());
        return itemData;
    }
    public int GetItemTotal(string type)
    {
        //Debug.Log("GetItemTotal");
        ItemData itemData = dataModel.ReadDictionary<ItemData>(DataPath.ITEM, type);
        int total = itemData.total;
        //Debug.Log($"TOTAL ITEM{itemData.id} {total}");
        return total;
    }
    public void AddItemTotal(string type, int inTotal)
    {
        Debug.Log("DATA === ADD ITEMDATA");
        inTotal += GetItemTotal(type);
        SetItemTotal(type, inTotal);
    }
    public void SetItemTotal(string type, int inTotal)
    {
        Debug.Log("DATA === SAVE ITEMDATA");
        ItemData itemData = new ItemData
        {
            id = type,
            total = inTotal,
        };
        Debug.Log("ITEM DATA" + itemData);
        //SaveHighestLevel(id);
        dataModel.UpdateDataDictionary(DataPath.ITEM, type.ToString(), itemData);
    }
    public void SaveGold(int gold, Action callback)
    {
        dataModel.UpdateData(DataPath.GOLD, gold,()=>
        {
            callback?.Invoke();
        }); 
    }
    #endregion
}
