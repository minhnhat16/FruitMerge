using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    [SerializeField]
    public UserInfo userInfo;
    [SerializeField]
    public ItemData itemData;
    [SerializeField]
    public UserInventory inventory;
    [SerializeField]
    public UserLevelData levelData;
}
[Serializable]
public class UserInfo
{
    public int ID;
    public string name;
}
[Serializable]
public class UserInventory
{
    public int currentFruitSkinID;
    public int currentBoxSkinID;
    [SerializeField]
    public Dictionary<string, DailyData> dailyData = new Dictionary<string, DailyData>();
    [SerializeField]
    public Dictionary<string, ItemData> itemInventory = new Dictionary<string, ItemData>();
    public List<int> fruitskinOwned = new();
    public List<int> boxSkinOwned = new();
    public int gold;
    public string lastCheckedData;
}
[Serializable]
public class DailyData
{
    public int day;
    public IEDailyType type;
}
[Serializable]
public class ItemData
{
    public string id;
    public int total;
}
[Serializable]
public class UserLevelData
{
    public int highestScore;
    public int currentRank;
    public int highestRank;
}

