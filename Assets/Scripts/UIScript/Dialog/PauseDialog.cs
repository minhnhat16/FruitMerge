using System;
using UnityEngine;

public class PauseDialog : BaseDialog
{

    public void PlayButton()
    {
        IngameController.instance.isPause = false;

        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog, () =>
        {
            //EndlessLevel.Instance.RandomCircle();
        });

    }
    public void HomeButton()
    {
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
                CameraMain.instance.main.gameObject.SetActive(false);

                Debug.Log("LOAD SCENE BUFFER FROM QUIT");
                ViewManager.Instance.SwitchView(ViewIndex.MainScreenView, null, () =>
                {
                    EndlessLevel.Instance.Clear();
                });
        });

       

    }
    public void RestartButton()
    {
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
        //GridSystem.instance.Reset();
        //GridSystem.instance.Init();
        IngameController.instance.isPause = false;
    }
    public void CloseBtn()
    {
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
        IngameController.instance.isPause = false;

    }
}
