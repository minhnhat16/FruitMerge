using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
public class BootLoader : MonoBehaviour
{
    [SerializeField ]private GameManager gameManager;
    [SerializeField] private UIRootControlScale uiRoot;
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
            SkinLibControl.Instance.InitFruitSkin();
            MainScreenViewParam param = new MainScreenViewParam();
            param.totalGold = DataAPIController.instance.GetGold();
            LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
            {
                SoundManager.Instance.PlayMusic(SoundManager.Music.GamplayMusic);
                DayTimeController.instance.CheckNewDay();
                if (DayTimeController.instance.isNewDay)
                {
                    Debug.Log("isnewday now go to claim spin reward");
                    ViewManager.Instance.SwitchView(ViewIndex.SpinView);
                }
                else
                {
                    Debug.Log("still in last day can't claim spin reward");
                    DialogManager.Instance.ShowDialog(DialogIndex.LabelChooseDialog);
                }
            });
        });
    }
   
}
