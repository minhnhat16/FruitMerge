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
        param = (LoseDialogParam)dialogParam;
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        IngameController.instance.SwitchLoseCamOnOff(true);
        EndlessLevel.Instance.DespawnMainCircle();
        //boxCamera.transform.LookAt(IngameController.instance.Wall.transform);
        IngameController.instance.isPause = true;
        //score_lb.text = param.score.ToString();
        //EndlessLevel.Instance.main.gameObject.SetActive(false);
        //EndlessLevel.Instance.Clear();
    }
    public override void OnStartHideDialog()
    {
        base.OnEndHideDialog();
        IngameController.instance.SwitchLoseCamOnOff(true);
    }
    public void HomeBtn()
    {
        IngameController.instance.isGameOver = false;
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
    public void RankBtn()
    {
        //show rank dialog
    }

}
