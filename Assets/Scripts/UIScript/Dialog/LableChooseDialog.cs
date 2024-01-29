using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LableChooseDialog : BaseDialog
{
    public TabShop tabShop;
    public TabSkin tabSkin;
    public TabPlay tabPlay;
    public TabLeaderBoard tabLeaderBoard;
    public TextMeshProUGUI gold_lb;

    [HideInInspector]
    public UnityEvent<int> onGoldChanged = new UnityEvent<int>();
    private void OnEnable()
    {
        onGoldChanged = IngameController.instance.onGoldChanged;
        onGoldChanged.AddListener(GoldChange);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        gold_lb.text = DataAPIController.instance.GetGold().ToString();
    }
    public void AddGoldButton()
    {
        SelectShopTab();
    }
    public void GoldChange(int gold)
    {
        gold_lb.text = gold.ToString();
    }
    public void SelectShopTab()
    {
        tabShop.OnClickTabOn();
        tabPlay.OnTabOff();
        tabLeaderBoard.OnTabOff();
        tabSkin.OnTabOff();
    }
    public void SelectSkinTab()
    {
        tabSkin.OnClickTabOn();
        tabPlay.OnTabOff();
        tabShop.OnTabOff();
    }
    public void SelectPlayTab()
    {
        tabPlay.OnClickTabOn();
        tabShop.OnTabOff();
        tabLeaderBoard.OnTabOff();
        tabSkin.OnTabOff();
    }
    public void SelectLeadBoardTab()
    {
        tabLeaderBoard.OnClickTabOn();
        tabPlay.OnTabOff();
        tabShop.OnTabOff();
        tabSkin.OnTabOff();
    }

}
