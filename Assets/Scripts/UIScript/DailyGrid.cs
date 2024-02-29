using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DailyGrid : MonoBehaviour
{
    public static DailyGrid instance;
    public Dictionary<int, DailyItem> DailyItems;
    public DailyItem currentDaily;

    [HideInInspector] public UnityEvent<bool> newDateEvent = new UnityEvent<bool>();
    [HideInInspector] public UnityEvent<bool> resetDailyEvent = new UnityEvent<bool>();
    private void Awake()
    {
        instance=this;
    }
    private void OnEnable()
    {
        newDateEvent.AddListener(NewDayRewardRemain);
    }
    private void OnDisable()
    {
        newDateEvent.RemoveListener(NewDayRewardRemain);
    }
    // Start is called before the first frame update
    void Start()
    {
        DailyItems = new Dictionary<int, DailyItem>();
        SetupGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupGrid()
    {
        Debug.Log("SET UP GRID");
        for (int i = 0; i < 7; i++)
        {
            if (i < 6)
            {
                var dailyItem = Instantiate(Resources.Load("Prefab/UIPrefab/DailyItem", typeof(GameObject)), transform) as GameObject;
                if (dailyItem == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    DailyItems.Add(i, dailyItem.GetComponent<DailyItem>());
                    var dailyConfig = ConfigFileManager.Instance.DailyRewardConfig.GetRecordByKeySearch(i);
                    SetupDailyRewardItem(dailyItem.GetComponent<DailyItem>(), dailyConfig);
                }
            }
            else
            {
                var dailyItem = Instantiate(Resources.Load("Prefab/UIPrefab/LastDailyItem", typeof(GameObject)), transform) as GameObject;
                if (dailyItem == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    DailyItems.Add(i, dailyItem.GetComponent<DailyItem>());
                    var dailyConfig = ConfigFileManager.Instance.DailyRewardConfig.GetRecordByKeySearch(i);
                    SetupDailyRewardItem(dailyItem.GetComponent<DailyItem>(), dailyConfig);
                }
            }
        }
    }
    public void UpdateDailyReward(DailyItem item)
    {
        if(item == null)
        {
            Debug.Log(" Daily item == null");
        }
        int index = item.day + 1;
        DailyItems[index].SwitchItemType((IEDailyType.Available).ToString());
    }
    private void SetupDailyRewardItem(DailyItem dailyItem, DailyRewardConfigRecord dailyRewardConfig)
    {
        if (dailyRewardConfig == null)
        {
            Debug.Log("Null Config");
            return;
        }
        int day = dailyRewardConfig.ID + 1;
        //Debug.Log($"Key {day}");
        DailyData dailyData = DataAPIController.instance.GetDailyData(day.ToString());
        if (day == 1 && dailyData.type == IEDailyType.Unavailable)
        {
            Debug.Log("DEFAULT AVAILABLE TO CLAIM");
            dailyItem.Init(IEDailyType.Available, dailyRewardConfig.TotalItem, dailyRewardConfig.ID + 1, dailyRewardConfig.SpriteName, dailyRewardConfig.ItemName);
            currentDaily = dailyItem;
        }
        else if (dailyData.type == IEDailyType.Available)
        {
            Debug.Log("AVAILABLE TO CLAIM");
            dailyItem.Init(IEDailyType.Available, dailyRewardConfig.TotalItem, dailyRewardConfig.ID + 1, dailyRewardConfig.SpriteName, dailyRewardConfig.ItemName);
            currentDaily = dailyItem;
        }
        else
        {
            Debug.Log("ELSE AVAILABLE TO CLAIM " + dailyData.type);
            dailyItem.Init(dailyData.type, dailyRewardConfig.TotalItem, dailyRewardConfig.ID + 1, dailyRewardConfig.SpriteName, dailyRewardConfig.ItemName);
        }
    }
    bool Predicate(KeyValuePair<int, DailyItem> kvp)
    {
        // Define your custom search criteria here
        return kvp.Value.currentType == IEDailyType.Unavailable;
    }
    public DailyItem NewDayItemAvailable()
    {
        KeyValuePair<int, DailyItem> foundItem = DailyItems.FirstOrDefault(Predicate);
        if (!foundItem.Equals(default(KeyValuePair<int, DailyItem>)))
        {
            int key = foundItem.Key;
            DailyItem item = foundItem.Value;
            // Do something with 'key' and 'item'
            return item;
        }
        else
        {
            // Item not found based on the predicate
            Debug.Log("Item not found based on the search criteria.");
            return null;
        }
    }
    public void NewDayRewardRemain(bool isNewDay)
    {
        Debug.Log("NEW DAY REWARD REMAIN" + isNewDay);
        if (isNewDay)
        {
            Debug.Log("NEW DAY REWARD REMAIN");
            var newDayItem = NewDayItemAvailable();
            newDayItem.currentType = IEDailyType.Available;
            DataAPIController.instance.SetDailyData(newDayItem.day.ToString(), newDayItem.currentType);
            currentDaily = newDayItem;
        }
    }
}
