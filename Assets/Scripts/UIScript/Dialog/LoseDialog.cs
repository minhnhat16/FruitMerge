using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LoseDialog : BaseDialog
{
    [SerializeField] private Text score_lb;
    [SerializeField] private LoseDialogParam param;
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
        param = (LoseDialogParam)dialogParam;
    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        IngameController.instance.isGameOver = false;
        IngameController.instance.isPause = true;
        score_lb.text = param.score.ToString(); 
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
                        Player.instance.canDrop = true; 
                        IngameController.instance.ResetScore();
                    });
                });
            });
        });
        
      
    }

}
