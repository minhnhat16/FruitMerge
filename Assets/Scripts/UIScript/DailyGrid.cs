using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyGrid : MonoBehaviour
{
    public Dictionary<int, DailyItem> DailyItems;
    [SerializeField] private List<DailyItem> _items;
    [SerializeField] private GridLayoutGroup _content;
    public DailyItem currentDaily;
    public bool isNewDay;
    [HideInInspector] public UnityEvent<bool> newDateEvent = new UnityEvent<bool>();
    [HideInInspector] public UnityEvent<bool> resetDailyEvent = new UnityEvent<bool>();
    [HideInInspector] public UnityEvent<bool> lastItemClaimEvent = new UnityEvent<bool>();
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
        //DayTimeController.instance.CheckNewDay();
        isNewDay = DayTimeController.instance.isNewDay;
        DailyItems = new Dictionary<int, DailyItem>();
        SetupGrid();
        CheckFullDailyClaim();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNewDay)
        {
            newDateEvent?.Invoke(isNewDay);
        }
    }
    public void SetupGrid()
    {
        if (DailyItems.Count != 0)
        {
            Debug.Log("SETUP GRID " + DailyItems.Count);
            return;
        }
        for (int i = 0; i < 7; i++)
        {
            if (i < 6)
            {
                var gridChildren = GetComponentInChildren<GridLayoutGroup>().gameObject;
                if (gridChildren == null) Debug.LogError("GRID CHILDREN NULL");
                var dailyItem = Instantiate(Resources.Load("Prefab/UIPrefab/DailyItem", typeof(GameObject)), gridChildren.transform) as GameObject;
                if (dailyItem == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    DailyItems.Add(i, dailyItem.GetComponent<DailyItem>());
                    var dailyConfig = ConfigFileManager.Instance.DailyRewardConfig.GetRecordByKeySearch(i);
                    SetupDailyRewardItem(dailyItem.GetComponent<DailyItem>(), dailyConfig);
                    _items.Add(dailyItem.GetComponent<DailyItem>());
                }
            }
            else
            {
                var gridChildren = GetComponentInChildren<GridLayoutGroup>().gameObject;
                var dailyItem = Instantiate(Resources.Load("Prefab/UIPrefab/LastDailyItem", typeof(GameObject)), gridChildren.transform) as GameObject;
                if (dailyItem == null)
                {
                    Debug.LogError(" item == null");
                }
                else
                {
                    DailyItems.Add(i, dailyItem.GetComponent<DailyItem>());
                    var dailyConfig = ConfigFileManager.Instance.DailyRewardConfig.GetRecordByKeySearch(i);
                    SetupDailyRewardItem(dailyItem.GetComponent<DailyItem>(), dailyConfig);
                    _items.Add(dailyItem.GetComponent<DailyItem>());
                }
            }
        }
    }
    public void InvokeWhenHaveCurrentDaily()
    {
        if (currentDaily != null && currentDaily.currentType == IEDailyType.Available) currentDaily.CheckItemAvailable();
        else
        {
            var item = _items.First((DailyItem daily) => daily.currentType == IEDailyType.Unavailable);
            item.CheckItemAvailable();
        }
    }
    public void UpdateDailyReward(DailyItem item)
    {
        if (item == null)
        {
            Debug.Log(" Daily item == null");
        }
        int index = item.day + 1;
        DailyItems[index].SwitchItemType((IEDailyType.Available).ToString());
    }
    private void CheckFullDailyClaim()
    {
        //check daily in day seven is claimed;
        var lastDailyData = DataAPIController.instance.GetDailyData("7");
        if (lastDailyData.type == IEDailyType.Claimed)
        {
            DataAPIController.instance.SetNewDailyCircle();
        }
    }
    private void SetupDailyRewardItem(DailyItem dailyItem, DailyRewardConfigRecord dailyRewardConfig)
    {
        if (dailyRewardConfig == null) return;
        int day = dailyRewardConfig.ID + 1;
        DailyData dailyData = DataAPIController.instance.GetDailyData(day.ToString());
        if (dailyData.type == IEDailyType.Available)
        {
            Debug.Log("AVAILABLE TO CLAIM");
            dailyItem.Init(IEDailyType.Available, dailyRewardConfig.TotalItem, dailyRewardConfig.ID + 1, dailyRewardConfig.SpriteName, dailyRewardConfig.ItemName);
            currentDaily = dailyItem;
        }
        else
        {
            Debug.Log("ELSE UNAVAILABLE TO CLAIM " + dailyData.type);
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
            this.isNewDay = false;
            Debug.Log("NEW DAY REWARD REMAIN");
            var newDayItem = NewDayItemAvailable();
            Debug.Log($"new day item id {newDayItem.day}");
            newDayItem.SwitchType(IEDailyType.Available);
            DataAPIController.instance.SetDailyData(newDayItem.day.ToString(), newDayItem.currentType);
            currentDaily = newDayItem;
            currentDaily.CheckItemAvailable();
        }
        else
        {
        }
    }
}
