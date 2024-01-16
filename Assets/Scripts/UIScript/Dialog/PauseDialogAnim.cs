using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDialogAnim : BaseDialogAnimation
{
    public Animator animator;
    private Action callback;
    public override void HideDialogAnimation(Action callback)
    {
        this.callback = callback;
        Debug.Log("HideDialogAnimation");
        animator.Play("PauseDialogHide");
    }

    public override void ShowDialogAnimation(Action callback)
    {
        this.callback = callback;
        Debug.Log("ShowDialogAnimation");
        animator.Play("PauseDialogShow");
    }

    public void ShowAnim()
    {
        callback?.Invoke();
    }

    public void HideAnim()
    {
        callback?.Invoke();
    }
    public void Clear()
    {
        callback?.Invoke();
    }
}
