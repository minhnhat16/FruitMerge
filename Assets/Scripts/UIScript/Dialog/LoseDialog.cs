using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoseDialog : BaseDialog
{
    [SerializeField] private TextMeshProUGUI score_lb;
    [SerializeField] private LoseDialogParam param;
    [SerializeField] private Camera boxCamera;
    private void Start()
    {
        param = new LoseDialogParam();
        score_lb = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        EndlessLevel.Instance.SetActiveMainCircle(false);
        score_lb.text = "Score: " + IngameController.instance.Score.ToString();
        param.score = IngameController.instance.Score;
        IngameController.instance.isPause = true;
    }   
    public override void OnEndShowDialog()
    {
        base.OnEndShowDialog();

    }
    public override void OnStartHideDialog()
    {
        base.OnEndHideDialog();
        IngameController.instance.SwitchLoseCamOnOff(false);
        EndlessLevel.Instance.Clear();
        CirclePool.instance.pool.DeSpawnAll();
    }
    public override void OnEndHideDialog()
    {
        base.OnEndHideDialog();
        ZenSDK.instance.ShowFullScreen();
    }
    public void HomeBtn()
    {

        IngameController.instance.isGameOver = false;
        DialogManager.Instance.HideDialog(dialogIndex);
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
            ZenSDK.instance.OnGameOver(param.score.ToString());
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
            EndlessLevel.Instance.Clear();
            LoadSceneManager.instance.LoadSceneByName("Ingame", () =>
            {
                ViewManager.Instance.SwitchView(ViewIndex.GamePlayView, null, () =>
                {
                    EndlessLevel.Instance.SetLife = 1;
                    IngameController.instance.LoseCam.gameObject.SetActive(false);
                    IngameController.instance.ResetScore();
                    IngameController.instance.SetUpIngame();
                    IngameController.instance.isPause = false;
                    IngameController.instance.isGameOver = false;
                });
            });
        });
    }
 

}
