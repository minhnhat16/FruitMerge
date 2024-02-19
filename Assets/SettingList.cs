using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingList : MonoBehaviour
{
    public Animator animator;
    private Action callback;
    public void HideSettingList(Action callback)
    {
        this.callback = callback;
        Debug.Log("HideSettingList");
        animator.Play("PauseListHide");
    }

    public void ShowSettingList(Action callback)
    {
        this.callback = callback;
        Debug.Log("ShowSettingList");
        animator.Play("PauseListAnim");   
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
