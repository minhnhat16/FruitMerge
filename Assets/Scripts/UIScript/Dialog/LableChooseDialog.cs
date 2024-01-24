using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LableChooseDialog : BaseDialog
{
    public TabShop tabShop;
    public TabSkin tabSkin;
    public TabPlay tabPlay;
    public TabLeaderBoard tabLeaderBoard;
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
