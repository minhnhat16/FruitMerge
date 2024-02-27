using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DailyGrid : MonoBehaviour
{
    public Dictionary<int, DailyItem> DailyItems;
    public DailyItem currendDaily;
    private void OnEnable()
    {

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
    private void SetupDailyRewardItem(DailyItem dailyItem, DailyRewardConfigRecord dailyRewardConfig)
    {
        if (dailyRewardConfig == null)
        {
            Debug.Log("Null Config");
            return;
        }
        int day = dailyRewardConfig.ID + 1;
        Debug.Log($"Key {day}");
        DailyData dailyData = DataAPIController.instance.GetDailyData(day.ToString());
        if (day == 1 && dailyData.type == IEDailyType.Unavailable)
        {
            dailyItem.Init(IEDailyType.Available, dailyRewardConfig.TotalItem, dailyRewardConfig.ID + 1, dailyRewardConfig.SpriteName, dailyRewardConfig.ItemName);
        }
        else
        {
            dailyItem.Init(dailyData.type, dailyRewardConfig.TotalItem, dailyRewardConfig.ID + 1, dailyRewardConfig.SpriteName, dailyRewardConfig.ItemName);
        }
    }
}
