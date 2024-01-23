using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScreenView : BaseView
{
    public int totalGold;
    public TextMeshProUGUI gold_lb;
    public TabPlay tabPlay;
    public TabShop tabShop;
    public TabLeaderBoard tabLeaderBoard;
    public override void Setup(ViewParam viewParam)
    {
        base.Setup(viewParam);
        if(viewParam != null)
        {
            MainScreenViewParam param = viewParam as MainScreenViewParam;
            totalGold = param.totalGold;
            gold_lb.text = totalGold.ToString();

            DataTrigger.RegisterValueChange(DataPath.GOLD, GoldValueChange);
        }

    }

    private void GoldValueChange(object data)
    {
        int gold = (int)data;
        totalGold = gold; 
        gold_lb.text = totalGold.ToString();    
    }

    public override void OnStartShowView()
    {
        base.OnStartShowView();
    }
    public void  SelectShopTab()
    {
        tabShop.OnClickTabOn();
        tabPlay.OnTabOff();
        tabLeaderBoard.OnTabOff();
    }
    public void SelectPlayTab()
    {
        tabPlay.OnClickTabOn();
        tabShop.OnTabOff();
        tabLeaderBoard.OnTabOff();
    }
    public void SelectLeadBoardTab()
    {
        tabLeaderBoard.OnClickTabOn();
        tabPlay.OnTabOff();
        tabShop.OnTabOff();
    }
}
