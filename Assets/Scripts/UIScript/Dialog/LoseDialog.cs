using System;
using UnityEngine;
using UnityEngine.UI;

public class LoseDialog : BaseDialog
{
    [SerializeField] private Text score_lb;
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        IngameController.instance.isGameOver = false;
        IngameController.instance.isPause = true;
        //EndlessLevel.Instance.Clear();
    }
    public void HomeBtn()
    {

        DialogManager.Instance.HideAllDialog();

        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
            ViewManager.Instance.SwitchView(ViewIndex.MainScreenView, null, () =>
            {
                IngameController.instance.isPause = true;
            });
        });
    }
    public void RePlayBtn()
    {
        DialogManager.Instance.HideDialog(DialogIndex.LoseDialog, () =>
        {
            IngameController.instance.SetUpLevel();
            IngameController.instance.isPause = false;

        });
    }
}
