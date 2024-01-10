using System;
using System.Collections;
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
    [SerializeField]
    public Dictionary<string, ItemData> itemInventory = new Dictionary<string, ItemData>();
    public int gold;
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

