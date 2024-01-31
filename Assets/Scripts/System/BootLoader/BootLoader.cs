using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
public class BootLoader : MonoBehaviour
{
    private GameManager gameManager;
    
    IEnumerator Start()
    {
        DontDestroyOnLoad(this);
        DOTween.SetTweensCapacity(1000, 50);
        yield return new WaitForSeconds(0.5f);
        gameManager = GetComponentInChildren<GameManager>();
        InitDataDone(() =>
        {
            InitConfig();
        });
    }
    private void InitDataDone(Action callback)
    {
        DataAPIController.instance.InitData(() =>
        {
            callback?.Invoke();
        });
    }
    private void InitConfig()
    {
        ConfigFileManager.Instance.Init(() =>
        {
            gameManager.SetupGameManager();
            MainScreenViewParam param = new MainScreenViewParam();
            param.totalGold = DataAPIController.instance.GetGold();
            DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);

        });
    }
   
}
