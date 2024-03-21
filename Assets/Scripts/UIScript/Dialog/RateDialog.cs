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
        stars.StarsListOff();
        OnRateEvents(false);
        if (EndlessLevel.Instance == null) return;
        if (EndlessLevel.Instance.main != null)
        {

            EndlessLevel.Instance.SetActiveMainCircle(false);
        }
        if (IngameController.instance.player != null)
        {
            Player.instance.gameObject.SetActive(false);
        }
    }
    public override void OnEndHideDialog()
    {
        base.OnEndHideDialog();
        if (Player.instance != null)
        {
            Player.instance.canDrop = true;
            Player.instance.gameObject.SetActive(true);
        }
        EndlessLevel.Instance.main.gameObject.SetActive(true);
    }
    public void CloseButton()
    {
        DialogManager.Instance.HideDialog(dialogIndex, () =>
        {
            Debug.Log("CloseButton");
            Player.instance.canDrop = true;
        });
    }
    public void ReMindLaterBtn()
    {
        DialogManager.Instance.HideDialog(this.dialogIndex, () =>
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
        stars.StarListConfirm(() =>
        {
            rateAnim.PlaySuccesfullRating(() =>
            {
                DialogManager.Instance.HideDialog(dialogIndex, () =>
                {
                    Debug.Log("HIDE DIALOG " + dialogIndex);
                    rateBtn.gameObject.SetActive(false);
                });
            });
        });
       
    }
}
