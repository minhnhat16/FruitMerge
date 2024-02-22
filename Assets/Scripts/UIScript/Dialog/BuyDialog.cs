using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyDialog : BaseDialog
{
    [SerializeField] private  Text total_lb;
    [SerializeField] private Text amount_lb;
    [SerializeField] private Text cost_lb;
    [SerializeField] private Text explain_lb;
    private Action onConfirm;
    private Action onCancel;

    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
        if (dialogParam != null)
        {
            BuyConfirmDialogParam param = (BuyConfirmDialogParam)dialogParam;
            onConfirm = param.onConfirmAction;
            onCancel = param.onCancleAction;
        }
    }

    public void HideConfirmDialog()
    {
        DialogManager.Instance.HideDialog(dialogIndex);
    }

    public void ConfirmBuy()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClickSFX);
        onConfirm?.Invoke();
        HideConfirmDialog();
    }

    public void CancleBuy()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClickSFX);
        onCancel?.Invoke();
        HideConfirmDialog();
    }
}
