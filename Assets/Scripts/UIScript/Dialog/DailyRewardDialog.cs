using UnityEngine;
using UnityEngine.Events;

public class DailyRewardDialog : BaseDialog
{
    public DailyClaimBtn claimBtn;
    public DailyGrid dailyGrid;

    [HideInInspector] public UnityEvent<bool> onClickDailyItem = new();
    [HideInInspector] public UnityEvent<bool> onClickClaim = new();
    [HideInInspector] public UnityEvent<bool> onClickAds = new();
    private void OnEnable()
    {
        onClickDailyItem.AddListener(ClickDailyItem);
        onClickClaim.AddListener(ClickClaimReward);
        onClickAds.AddListener(OnClickAdsReward);
        claimBtn.SetButtonEvent(onClickClaim, onClickAds);
    }
    private void OnDisable()
    {
        onClickDailyItem.RemoveListener(ClickDailyItem);
        onClickClaim.RemoveListener(ClickClaimReward);
        onClickAds.RemoveListener(OnClickAdsReward);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
    }
    private void Update()
    {
    }
    public void ClickDailyItem(bool isEnable)
    {
        claimBtn.enabled = true;
        if (isEnable)
        {
            Debug.Log($"Check Button type {isEnable}");
            claimBtn.CheckButtonType();
            claimBtn.gameObject.SetActive(true);
        }
        else
        {
            //claimBtn.enabled = false;
            Debug.Log($"Check Button type {isEnable}");
            claimBtn.gameObject.SetActive(false);
        }
    }
    public void ClickClaimReward(bool isClaim)
    {

        if (isClaim)
        {
            Debug.Log("claim reward successful");
            claimBtn.gameObject.SetActive(false);
            dailyGrid.currentDaily.ItemClaim(isClaim);
        }
    }
    public void OnClickAdsReward(bool isAds)
    {
        if (isAds)
        {
            Debug.Log("Ads reward showing");
        }
    }
    public void QuitButton()
    {
        DialogManager.Instance.HideDialog(dialogIndex);
    }
}
