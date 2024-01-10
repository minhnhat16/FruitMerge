using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Animator animator;
    private CanvasGroup canvasGroup;
    private Action callback;
    private Text text;
    private int score;
    public int scoreSpan = 1;
    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        text = GetComponent<Text>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
    }
    private void Update()
    {
    }
    public void CompareScore()
    {
         //Debug.Log("COMPARE SCORE" + score + "IN GAME" + EndlessLevel.Instance.score);
            ShowScoreAnim(() =>
            {
                score += scoreSpan;
                text.text = score.ToString();
            });
    }
    public void ShowScoreAnim(Action callback)
    {
        //Debug.Log("SHOW SCORE ANIM");
        this.callback = callback;
        animator.Play("ScoreChanged");
        callback?.Invoke();
    }
    public void ScoreChangeAnimation()
    {
        callback?.Invoke();
    }
}
