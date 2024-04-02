using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemConfirmDialog : BaseDialog
{
    private ItemType type;
    private bool isAds;
    [SerializeField] TextMeshProUGUI tutorial_lb;

    [SerializeField] Button ads;
    [SerializeField] Button confirm;

    public Button Ads { get { return ads; } set { this.ads = value; } }
    public Button Confirm { get { return confirm; } set { this.confirm = value; } }

    private void OnEnable()
    {
        ads.onClick.AddListener(() => { PlayAds(); });
        confirm.onClick.AddListener(() => { ConfirmUsingItem(); });
    }
    public override void Setup(DialogParam dialogParam)
    {
        base.Setup(dialogParam);
        if (dialogParam != null)
        {
            ItemConfirmParam param = (ItemConfirmParam)dialogParam;
            type = param.type;
            isAds = param.isAds;
            //name = param.name;
            //check item have enoughf to show ads btn
            if (isAds)
            {
                ads.gameObject.SetActive(true);
                confirm.gameObject.SetActive(false);
            }
            else //else show confirm to use
            {
                ads.gameObject.SetActive(true);
                confirm.gameObject.SetActive(false);
            }
        }

    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
        ItemCase(type);
        IngameController.instance.player.GetComponent<Player>().canDrop = false;
    }
    // Start is called before the first frame update
    public void PlayAds()
    {
        ZenSDK.instance.ShowVideoReward((isWatched) =>
        {
            if (isWatched)
            {
                DataAPIController.instance.SetItemTotal(type.ToString(),1);
                ConfirmUsingItem();
            }
            else 
            {
                Debug.LogWarning("Watch reward unsuccesfull");
            
            }
            ;
        });
    }
    public void ConfirmUsingItem()
    {
        switch (type)
        {
            case ItemType.CHANGE:
                Debug.Log("DESTROY ALL FRUIT BELOW 2");
                IngameController.instance.ChangeItem();
                IngameController.instance.CancelItem();
                DialogManager.Instance.HideDialog(DialogIndex.ItemConfirmDialog,null);

                break;
            case ItemType.HAMMER:
                Debug.Log("CHOSE ONE FRUIT TO DESTROY IT");
                IngameController.instance.BursItem();
                DialogManager.Instance.HideDialog(DialogIndex.ItemConfirmDialog, null) ;
                break;
            case ItemType.ROTATE:
                Debug.Log("CHOSE ONE FRUIT TO UPGRADE ");
                IngameController.instance.ShakeItem();
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
    void ItemCase(ItemType type)
    {
        switch (type)
        {
            case ItemType.CHANGE:
                tutorial_lb.text = "DESTROY ALL FRUIT BELOW 2";
                break;
            case ItemType.HAMMER:
                tutorial_lb.text = "CHOSE ONE FRUIT TO DESTROY IT";
                break;
            case ItemType.ROTATE:
                tutorial_lb.text = "CHOSE ONE FRUIT TO UPGRADE ";
                break;
        }
    }
}
