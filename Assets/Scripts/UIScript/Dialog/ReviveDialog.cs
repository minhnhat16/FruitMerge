using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ReviveDialog : BaseDialog
{
    [SerializeField] private TextMeshProUGUI score_lb;
    [SerializeField] private ReviveDialogParam param;
    [SerializeField] private Camera boxCamera;
    private void Start()
    {
    }   
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        score_lb.text = "Score: " + IngameController.instance.Score.ToString();
        EndlessLevel.Instance.SetActiveMainCircle(false);
        IngameController.instance.isPause = true;
        Player.instance.canDrop = false;
    }
    public override void OnEndShowDialog()
    {
        base.OnEndShowDialog();
    }
    public override void OnStartHideDialog()
    {
        base.OnEndHideDialog();
        IngameController.instance.SwitchLoseCamOnOff(false);
    }
    public override void OnEndHideDialog()
    {
        base.OnEndHideDialog();
        EndlessLevel.Instance.SetLife = 0;
    }
    public void RefuseBtn()
    {

        IngameController.instance.isGameOver = false;
        EndlessLevel.Instance.Clear();
        DialogManager.Instance.HideDialog(dialogIndex);
        IngameController.instance.SetIngameObjectActive(false); 
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
            DialogManager.Instance.ShowDialog(DialogIndex.LabelChooseDialog, null, () =>
            {
                IngameController.instance.isPause = false;
                CirclePool.instance.transform.localScale = Vector3.one;
                //WallScript.Instance.transform.localScale = Vector3.one;
            });
        });
    }
    public void ReviveButton()
    {
        DialogManager.Instance.HideDialog(DialogIndex.ReviveDialog, () =>
        {
            Player.instance.canDrop = true;
            IngameController.instance.isGameOver = false;   
            IngameController.instance.isPause = false;
            EndlessLevel.Instance.UnfreezeCircles();
            EndlessLevel.Instance.UsingChange();
            CirclePool.instance.transform.localScale = Vector3.one;
            WallScript.Instance.transform.localScale = Vector3.one;
            IngameController.instance.player.SetActive(true);
            WallScript.Instance.TopWallCouroutine();
            EndlessLevel.Instance.SetActiveMainCircle(true);

        });
    }
}
