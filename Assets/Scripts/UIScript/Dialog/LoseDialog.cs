using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoseDialog : BaseDialog
{
    [SerializeField] private Text score_lb;
    [SerializeField] private LoseDialogParam param;
    [SerializeField] private Camera boxCamera;
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        IngameController.instance.SwitchLoseCamOnOff(true);
        EndlessLevel.Instance.SetActiveMainCircle(false);
        IngameController.instance.isPause = true;
    }
    public override void OnStartHideDialog()
    {
        base.OnEndHideDialog();
        IngameController.instance.SwitchLoseCamOnOff(false);
        EndlessLevel.Instance.Clear();
        CirclePool.instance.pool.DeSpawnAll();
    }
    public void HomeBtn()
    {

        IngameController.instance.isGameOver = false;
        DialogManager.Instance.HideDialog(dialogIndex);
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {

            DialogManager.Instance.ShowDialog(DialogIndex.LabelChooseDialog,null, () =>
            {
                IngameController.instance.isPause = true;
                CirclePool.instance.transform.localScale = Vector3.one;
                WallScript.Instance.transform.localScale = Vector3.one;
            });
        });
    }
    public void RePlayBtn()
    {

        DialogManager.Instance.HideDialog(DialogIndex.LoseDialog, () =>
        {
            CirclePool.instance.transform.localScale = Vector3.one;
            WallScript.Instance.transform.localScale = Vector3.one;
            EndlessLevel.Instance.Clear();
            LoadSceneManager.instance.LoadSceneByName("Ingame", () =>
            {
                ViewManager.Instance.SwitchView(ViewIndex.GamePlayView, null, () =>
                {
                    EndlessLevel.Instance.LoadLevel(() =>
                    {
                        IngameController.instance.isPause = false;
                        IngameController.instance.isGameOver = false;
                        IngameController.instance.LoseCam.gameObject.SetActive(false);
                        Player.instance.gameObject.SetActive(true);
                        IngameController.instance.ResetScore();
                        WallScript.Instance.GetTopWall().SetActive(true);
                        Player.instance.canDrop = true;
                    });
                });
            });
        });
    }
 

}
