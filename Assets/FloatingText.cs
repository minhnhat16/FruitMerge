using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private Animator floatingAnim;
    Action callback;

    public void ShowFloatingText()
    {
        floatingAnim.Play("FloatingAnim");
        Debug.Log("play floating text");
    }
    public void ShowAnim()
    {
        callback?.Invoke();
    }
}
