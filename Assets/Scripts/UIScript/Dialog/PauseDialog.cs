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
        EndlessLevel.Instance.Clear();
        LoadSceneManager.instance.LoadSceneByName("Ingame", () =>
        {
            ViewManager.Instance.SwitchView(ViewIndex.GamePlayView, null, () =>
            {
                EndlessLevel.Instance.LoadLevel(() =>
                {
                    IngameController.instance.isPause = false;
                    IngameController.instance.ResetScore();
                });
            });
        });
    }
    public void CloseBtn()
    {
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
        IngameController.instance.isPause = false;

    }
}
