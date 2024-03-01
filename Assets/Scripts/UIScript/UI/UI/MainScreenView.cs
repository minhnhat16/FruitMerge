using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScreenView : BaseView
{
    //public int totalGold;
    //public TextMeshProUGUI gold_lb;

    public override void Setup(ViewParam viewParam)
    {
        base.Setup(viewParam);
      

    }
    public void Update()
    {
    }
    public void StartBtn()
    {
        GameManager.instance.LoadIngameSence();
    }

    public override void OnStartShowView()
    {
        base.OnStartShowView();
    }
    public void DailyRewardButton()
    {
        Debug.Log("Daily Reward Button");
        DialogManager.Instance.ShowDialog(DialogIndex.DailyRewardDialog);
    }

}
