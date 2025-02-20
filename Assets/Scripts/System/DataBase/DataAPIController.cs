
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        SaveGold(gold, null);
    }
    public void AddGold(int add)
    {
        int gold = dataModel.ReadData<int>(DataPath.GOLD);
        gold += add;
        SaveGold(gold, null);
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
    public void SaveCurrentLevel(int level)
    {
        dataModel.UpdateData(DataPath.CURRENTLV, level);
    }
    #region SKIN DATA
    public int GetFruitSkin(int id)
    {
        Debug.Log("DATA === FRUIT SKIN");
        int fruitSkin = dataModel.ReadData<int>(DataPath.FRUITSKIN);
        return fruitSkin;
    }
    public int GetCurrentFruitSkin()
    {
        int crFruitSkin = dataModel.ReadData<int>(DataPath.CURRENTFRUITSKIN); ;
        return crFruitSkin;
    }
    public void SetCurrenFruitSkin(int id, Action callback)
    {
        UnityAction<object> action = new UnityAction<object>((obj) => IngameController.instance.skinChanged?.Invoke((int)obj));
        DataTrigger.RegisterValueChange(DataPath.CURRENTFRUITSKIN,action);
        dataModel.UpdateData(DataPath.CURRENTFRUITSKIN, id, callback);
    }
    public int GetCurrentBoxSkin()
    {
        int crBoxSkin = dataModel.ReadData<int>(DataPath.CURRENTBOXSKIN);
        //Debug.Log("CR BOX SKIN " + crBoxSkin);
        return crBoxSkin;
    }
    public List<int> GetAllFruitSkinOwned()
    {
        List<int> ownedSkins = dataModel.ReadData<List<int>>(DataPath.FRUITSKIN);
        return ownedSkins;
    }
    public void SaveFruitSkin(int id)
    {
        var all = GetAllFruitSkinOwned();
        all.Add(id);
        dataModel.UpdateData(DataPath.FRUITSKIN, all);
    }
    //public void GetAllFruitSkin()
    //{
    //    dataModel.ReadData<FruitSkin>(DataPath.FRUITSKIN);
    //}
    #endregion
    #region daytimedata
    public string GetDayTimeData()
    {
        string day = dataModel.ReadData<string>(DataPath.DAYCHECKED);
        //Debug.Log($"day {day}");
        return day;
    }
    public void SetDayTimeData(string day)
    {
        if (!string.IsNullOrEmpty(day))
        {
            dataModel.UpdateData(DataPath.DAYCHECKED, day, () =>
             {
                 Debug.Log("SAVE DAYTIME DATA SUCCESSFULL");
             });
        }
    }
    #endregion
    #region Others
    public ItemData GetItemData(string type)
    {
        Debug.Log("DATA === ITEM DATA");
        ItemData itemData = dataModel.ReadDictionary<ItemData>(DataPath.ITEM, type);
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
        ItemData itemData = new()
        {
            id = type,
            total = inTotal,
        };
        dataModel.UpdateDataDictionary(DataPath.ITEM, type.ToString(), itemData);
    }

    public void SaveGold(int gold, Action callback)
    {
        dataModel.UpdateData(DataPath.GOLD, gold, () =>
         {
             callback?.Invoke();
         });
    }
    public Dictionary<string, DailyData> GetAllDailyData()
    {
        var dailyData = dataModel.ReadData<Dictionary<string, DailyData>>(DataPath.DAILYDATA);
        return dailyData;
    }
    public void SetNewDailyCircle()
    {
        for (int i = 1; i <= 7; i++)
        {
            DailyData dailyData = new();
            dailyData.day = i;
            dailyData.type = IEDailyType.Unavailable;
            dataModel.UpdateDataDictionary(DataPath.DAILYDATA, i.ToString(), dailyData);
        }
    }
    public DailyData GetDailyData(string key)
    {
        DailyData dailyData = dataModel.ReadDictionary<DailyData>(DataPath.DAILYDATA, key);
        return dailyData;
    }
    public void SetDailyData(string day, IEDailyType type)
    {
        DailyData dailyData = dataModel.ReadDictionary<DailyData>(DataPath.DAILYDATA, day);
        dailyData.type = type;
        dataModel.UpdateDataDictionary(DataPath.DAILYDATA, day, dailyData);
    }
    #endregion
}
