using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDayItem : DailyItem
{

    // Start is called before the first frame update
    private void OnEnable()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DebugButton()
    {
        Debug.Log("On Click Daily Item");
    }
    public override void OnClickDailyItem()
    {
        Debug.Log("On Click Daily Item");
        if (currentType == IEDailyType.Available)
        {
            //var parent = DialogManager.Instance.dicDialog[DialogIndex.DailyRewardDialog].GetComponent<DailyRewardDialog>();
            //   parent.dailyGrid.currentDaily = this;
            onClickDailyItem?.Invoke(true);
        }
        else
        {
            onClickDailyItem?.Invoke(false);
        }
    }
}
