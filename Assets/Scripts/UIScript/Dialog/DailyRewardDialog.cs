using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DailyRewardDialog : BaseDialog
{
    public DailyClaimBtn clamBtn;
    public DailyGrid dailyGrid;

    [HideInInspector] public UnityEvent<bool> onClickDailyItem = new();
    [HideInInspector] UnityEvent<bool> onClickClaim = new();
    [HideInInspector] UnityEvent<bool> onClickAds = new();
    private void OnEnable()
    {
        onClickDailyItem.AddListener(ClickDailyItem);
    }
    public void ClickDailyItem(bool isEnable)
    {
        if (isEnable)
        {
            clamBtn.enabled = true;
        }
        else { clamBtn.enabled = false;}
    }

}
