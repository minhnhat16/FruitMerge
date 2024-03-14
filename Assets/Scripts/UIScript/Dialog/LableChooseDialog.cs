using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    private void OnDisable()
    {
        onGoldChanged.RemoveListener(GoldChange);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        gold_lb.text = DataAPIController.instance.GetGold().ToString();
        SelectPlayTab();
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
        PauseDialogOff();

    }
    public void SelectSkinTab()
    {
        tabSkin.OnClickTabOn();
        tabLeaderBoard.OnTabOff();
        tabPlay.OnTabOff();
        tabShop.OnTabOff();
        PauseDialogOff();

    }
    public void SelectPlayTab()
    {
        tabPlay.OnClickTabOn();
        tabShop.OnTabOff();
        tabLeaderBoard.OnTabOff();
        tabSkin.OnTabOff();
        PauseDialogOff();
    }
    public void SelectLeadBoardTab()
    {
        tabLeaderBoard.OnClickTabOn();
        tabPlay.OnTabOff();
        tabShop.OnTabOff();
        tabSkin.OnTabOff();
        PauseDialogOff();
    }
    public void SettingDialogButton()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.SettingDialog, null);
    }
    public void DailyDialogButton()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.DailyRewardDialog, null);

    }
    public void PauseDialogOff()
    {
        DialogManager.Instance.HideDialog(DialogIndex.SettingDialog);
    }
}
