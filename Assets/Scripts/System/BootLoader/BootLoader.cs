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
        yield return new WaitForSeconds(0.2f);
        InitDataDone(() =>
        {
            DOTween.SetTweensCapacity(1000, 50);
            gameManager = GetComponentInChildren<GameManager>();
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
            LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
            {
                if (DayTimeController.instance.isNewDay)
                {
                    ViewManager.Instance.SwitchView(ViewIndex.SpinView);
                }
                else
                {
                    DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);
                }
            });
        });
    }
   
}
