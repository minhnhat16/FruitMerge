using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpin : MonoBehaviour
{
    public Animator animator;
    [SerializeField] Action callback;
    private void Awake()
    {
    }
    public void StopArrowAnim( Action callback)
    {
        this.callback = callback;
        animator.Play("ArrowHide");
    }
    public void PlayArrowAnim(Action callback)
    {
        this.callback = callback;
        animator.Play("ArrowPlay");
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
