using System;
using UnityEngine;

public class ShopDialogAnim : BaseDialogAnimation
{
    public Animator animator;
    private Action callback;
    public override void HideDialogAnimation(Action callback)
    {
        this.callback = callback;
        Debug.Log("HideDialogAnimation");
        animator.Play("ShopDialogHide");
    }

    public override void ShowDialogAnimation(Action callback)
    {
        this.callback = callback;
        Debug.Log("ShowDialogAnimation");
        animator.Play("ShopeDialogShow");
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
