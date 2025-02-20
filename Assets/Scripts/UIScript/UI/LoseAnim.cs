using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseAnim : BaseDialogAnimation
{
    public Animator animator;
    private Action callback;
    public override void HideDialogAnimation(Action callback)
    {
        this.callback = callback;
        animator.Play("LoseHide");
    }

    public override void ShowDialogAnimation(Action callback)
    {
        this.callback = callback;
        animator.Play("LoseShow");
    }

    public void ShowAnim()
    {
        callback?.Invoke();
    }

    public void HideAnim()
    {
        callback?.Invoke();
    }
    public void SetLoseCamOn()
    {
        Debug.Log("Clear");
        IngameController.instance.SwitchLoseCamOnOff(true);
    }
    public void SetLoseCamOff()
    {
        IngameController.instance.SwitchLoseCamOnOff(false);
    }
}
