using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RateDialog : BaseDialog
{
    [SerializeField] StarList stars;
    [SerializeField] Button rateBtn;
    [SerializeField] Button remindBtn;

    [HideInInspector]
    public UnityEvent<bool> rateEvent = new UnityEvent<bool>();

    private void OnEnable()
    {
        rateEvent.AddListener(OnRateEvents);
    }
    private void OnDisable()
    {
        rateEvent.RemoveListener(OnRateEvents);
    }
    private void Awake()
    {
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        OnRateEvents(false);
    }
    public void CloseButton()
    {
        DialogManager.Instance.HideDialog(DialogIndex.RateDialog, () =>
        {
            Debug.Log("CloseButton");

            if (Player.instance != null)
            {
                Player.instance.canDrop = true;
            }
        });
    }
    public void ReMindLaterBtn()
    {
        DialogManager.Instance.HideDialog(DialogIndex.RateDialog, () =>
        {
            Debug.Log("ReMindLaterBtn");
            if (Player.instance != null)
            {
                Player.instance.canDrop = true;
            }

        });
    }
    public void OnRateEvents(bool isRate)
    {
        rateBtn.gameObject.SetActive(isRate);
        remindBtn.gameObject.SetActive(!isRate);
    }
    public void RateButton()
    {
        Debug.Log("RateButton");
        var rateAnim = GetComponentInChildren<RateAnim>();
        rateAnim.PlaySuccesfullRating(() =>
        {
            DialogManager.Instance.HideDialog(DialogIndex.RateDialog, () =>
            {
            });
        });
    }
}
