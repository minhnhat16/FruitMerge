using System;
using UnityEngine;

public class LeaderBoardAnim : BaseDialogAnimation
{
    public Animator animator;
    private Action callback;
    public override void HideDialogAnimation(Action callback)
    {
        this.callback = callback;
        Debug.Log("HideDialogAnimation");
        animator.Play("LeaderBoardDialogHide");
    }

    public override void ShowDialogAnimation(Action callback)
    {
        this.callback = callback;
        Debug.Log("ShowDialogAnimation");
        animator.Play("LeaderBoardDialogShow");
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
