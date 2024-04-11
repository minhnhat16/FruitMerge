using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingView : BaseView
{
    public Image loadingProgress;
    public Text loaddingText;
    private float t1 = 0;

    public override void Setup(ViewParam viewParam)
    {
        base.Setup(viewParam);
    }
    public override void OnStartShowView()
    {
        base.OnStartShowView();
        //string sceneName = SceneManager.GetActiveScene().name;
        //if (sceneName == "InGame")
        //{
        //    GameManager.instance.SetUpIngame(); 
        //}
    }
    private void Update()
    {
        UpdateLoadingProgress();
    }

    private void UpdateLoadingProgress()
    {
        loadingProgress.fillAmount = 1 - LoadSceneManager.instance.progress;

        t1 += Time.deltaTime;
        if (t1 >= 0.2f && t1 < 0.4f)
        {
            loaddingText.text = "Loading..";
        }
        else if (t1 >= 0.4f && t1 < 0.6f)
        {
            loaddingText.text = "Loading...";
        }
        else if (t1 >= 0.6f)
        {
            loaddingText.text = "Loading.";
            t1 = 0;
        }
    }
}
