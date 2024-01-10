public class LoseDialog : BaseDialog
{
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        IngameController.instance.isGameOver = false;
        IngameController.instance.isPause = true;
        EndlessLevel.Instance.Clear();
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
