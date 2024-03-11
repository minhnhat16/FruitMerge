using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemConfirmDialog : BaseDialog
{
    private int type;
    [SerializeField] TextMeshProUGUI tutorial_lb;
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
        if (dialogParam != null)
        {
            ItemConfirmParam param = (ItemConfirmParam)dialogParam;
            type = param.type;
            name = param.name;
        }

    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        ItemCase(type);
        IngameController.instance.player.GetComponent<Player>().canDrop = false;
    }
    // Start is called before the first frame update

    public void ConfirmUsingItem()
    {
        switch (type)
        {
            case 0:
                Debug.Log("DESTROY ALL FRUIT BELOW 2");
                IngameController.instance.TomatoItem();
                IngameController.instance.CancelItem();
                DialogManager.Instance.HideDialog(DialogIndex.ItemConfirmDialog,null);

                break;
            case 1:
                Debug.Log("CHOSE ONE FRUIT TO DESTROY IT");
                IngameController.instance.BombItem();
                DialogManager.Instance.HideDialog(DialogIndex.ItemConfirmDialog, null) ;
                break;
            case 2:
                Debug.Log("CHOSE ONE FRUIT TO UPGRADE ");
                IngameController.instance.UpgradeItem();
                DialogManager.Instance.HideDialog(DialogIndex.ItemConfirmDialog, null   );
                break;
        }
    }
    public void CancelUsingItem()
    {
        IngameController.instance.CancelItem();
        DialogManager.Instance.HideDialog(dialogIndex, () =>
        {
            Player.instance.canDrop = true;
        });
    }
    void ItemCase(int type)
    {
        switch (type)
        {
            case 0:
                tutorial_lb.text = "DESTROY ALL FRUIT BELOW 2";
                break;
            case 1:
                tutorial_lb.text = "CHOSE ONE FRUIT TO DESTROY IT";
                break;
            case 2:
                tutorial_lb.text = "CHOSE ONE FRUIT TO UPGRADE ";
                break;
        }
    }
}
