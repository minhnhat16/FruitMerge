using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePlayView : BaseView, IPointerClickHandler
{
    [HideInInspector] GamePlayAnim anim;
    [SerializeField] TextMeshProUGUI score_rb;
    [SerializeField] private int _changeGold;
    [SerializeField] private int _curGold;
    [SerializeField] private Image nextBlock;
    [SerializeField] private Text nextValue;
    [SerializeField] private Text tomato_lb;
    [SerializeField] private Text bomb_lb;
    [SerializeField] private Text upgrade_lb;
    [SerializeField] private Text hammerAll_lb;
    [SerializeField] private Animator goldAnim;
    [SerializeField] private GameObject settingList;

    [SerializeField] Button tomato_Btn;
    [SerializeField] Button bomb_Btn;
    [SerializeField] Button upgrade_Btn;
    [SerializeField] Button clear_btn;

    public Text gold_lb;

    [SerializeField] bool settingActive;
    [SerializeField] bool onTomato;
    [SerializeField] bool onBomb;
    [SerializeField] bool onUpgrade;
    [SerializeField] List<Image> exit_img;
    [HideInInspector]
    public UnityEvent<int> setNextCircleEvent = new();
    [HideInInspector]
    public UnityEvent<int> setScoreEvent = new();
    [HideInInspector]
    public UnityEvent<int> tomatoItemEvent = new();
    [HideInInspector]
    public UnityEvent<int> bombItemEvent = new();
    [HideInInspector]
    public UnityEvent<int> upgradeItemEvent = new();
    [HideInInspector]
    public UnityEvent<bool> cancleItemEvent = new();
    private void OnEnable()
    {
        //setGoldTextEvent = GridSystem.instance.setGoldTextEvent;
        //setGoldTextEvent.AddListener(ShowGoldAnim);
        setNextCircleEvent = IngameController.instance.setNextCircleEvent;
        setNextCircleEvent.AddListener(NextCircleImage);
        setScoreEvent = IngameController.instance.setScoreEvent;
        setScoreEvent.AddListener(ScoreChange);
        tomatoItemEvent = IngameController.instance.tomatoItemEvent;
        tomatoItemEvent.AddListener(ChangeItem);
        bombItemEvent = IngameController.instance.bombItemEvent;
        bombItemEvent.AddListener(HammerItem);
        upgradeItemEvent = IngameController.instance.upgradeItemEvent;
        upgradeItemEvent.AddListener(ShakeItem);
        cancleItemEvent = IngameController.instance.cancleItemEvent;
        cancleItemEvent.AddListener(CancelItem);

    }
    private void OnDisable()
    {
        setScoreEvent.RemoveListener(ScoreChange);
        setNextCircleEvent.RemoveListener(NextCircleImage);
        tomatoItemEvent.AddListener(ChangeItem);
        bombItemEvent.AddListener(HammerItem);
        upgradeItemEvent.AddListener(ShakeItem);
    }
    public override void OnStartShowView()
    {
        base.OnStartShowView();
        onTomato = false;
        onBomb = false;
        onUpgrade = false;
        int total = DataAPIController.instance.GetItemTotal(ItemType.CHANGE.ToString());
        tomato_lb.text = CheckTotalItem(total);
        total = DataAPIController.instance.GetItemTotal(ItemType.HAMMER.ToString());
        bomb_lb.text = CheckTotalItem(total);
        total = DataAPIController.instance.GetItemTotal(ItemType.ROTATE.ToString());
        upgrade_lb.text = CheckTotalItem(total);
        int tracker = GameManager.instance.TrackLevelStart++;
        ZenSDK.instance.TrackLevelStart(tracker);
    }
    public string CheckTotalItem(int total)
    {
        if (total > 0) return total.ToString();
        else return "0";
    }
    public override void OnStartHideView()
    {
        base.OnStartHideView();
        anim = GetComponent<GamePlayAnim>();
    }
    public override void Setup(ViewParam viewParam)
    {
        base.Setup(viewParam);
        //int gold = DataAPIController.instance.GetGold();
        //_curGold = gold;
        //gold_lb.text = _curGold.ToString();
    }
    private void Update()
    {
        CheckExitImage();
    }
    public void CheckExitImage()
    {
        if (onBomb && onTomato && onUpgrade)
        {
            foreach (var e in exit_img)
            {
                e.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var e in exit_img)
            {
                e.gameObject.SetActive(false);
            }
        }
    }
    public void ShowGoldAnim(int gold)
    {
        _changeGold = gold;
        int calGold = _changeGold - _curGold;
        if (calGold == 0) return;

        _curGold = _changeGold;
        Debug.LogWarning("GOLD SHOW ANIM");
        goldAnim.gameObject.SetActive(true);
        goldAnim.Play("GoldAddShow");
        gold_lb.text = gold.ToString();

    }
    public void ScoreChange(int score)
    {
        score_rb.text = score.ToString();
    }
    public void PauseButton()
    {
        if (settingActive == true)
        {
            Debug.Log("pause button list ON");
            settingList.GetComponent<SettingList>().HideSettingList(() =>
            {
                settingActive = false;
                settingList.SetActive(false);
                clear_btn.gameObject.SetActive(true);
                Player.instance.canDrop = true;
            });
        }
        else
        {
            settingActive = true;
            settingList.SetActive(true);
            clear_btn.gameObject.SetActive(true);
            settingList.GetComponent<SettingList>().ShowSettingList(null);
            Player.instance.canDrop = false;


        }
    }
    public void NextCircleImage(int id)
    {
        id--;
        nextBlock.transform.DOScale(0.1f, 0);
        Tween tween = nextBlock.transform.DOScale(0.65f, 0.25f).SetEase(Ease.OutBounce);
        int skinID = DataAPIController.instance.GetCurrentFruitSkin();
        var name = SpriteLibControl.Instance.GetCircleSpriteName(skinID, id);
        var sprite = SpriteLibControl.Instance.GetSpriteByName(name);
        nextBlock.sprite = sprite;
        tween.OnComplete(() =>
        {
            tween?.Kill();
        });
    }
    public void ItemUsing(ItemType type)
    {
        int total = DataAPIController.instance.GetItemTotal(type.ToString());
        if (!onTomato && !onUpgrade && !onBomb)
        {
            CancelItem(false);
        }
        else if (onTomato && onUpgrade && onBomb) { }
        {
            ItemConfirmParam param = new();
            param.type = type;

            if (total > 0 && EndlessLevel.Instance._Circles.Count != 0)
            {
                param.isAds = true;
                DialogManager.Instance.ShowDialog(DialogIndex.ItemConfirmDialog, param, () =>
                {
                    onTomato = true;
                    onUpgrade = true;
                    onBomb = true;
                });
            }
            else if (total <= 0)
            {
                param.isAds = false;
                DialogManager.Instance.ShowDialog(DialogIndex.ItemConfirmDialog, param, () =>
                {
                    onTomato = true;
                    onUpgrade = true;
                    onBomb = true;
                });
            };
        }
    }
    public void CancelItem(bool onUse)
    {
        onTomato = onUse;
        onUpgrade = onUse;
        onBomb = onUse;
        Player.instance.canDrop = true;
        EndlessLevel.Instance.DisableTargetCircles();
    }
    public void OnClickChange()
    {
        onTomato = !onTomato;
        Player.instance.canDrop = false;
        if (onTomato == false)
        {
            CancelItem(false);
        }
        else
        {
            ItemUsing(ItemType.CHANGE);
        }
    }
    public void OnClickHammer()
    {
        onBomb = !onBomb;
        Player.instance.canDrop = false;
        if (onBomb == false)
        {
            CancelItem(false);
        }
        else
        {
            ItemUsing(ItemType.HAMMER);
        }
    }
    public void OnClickShake()
    {
        onUpgrade = !onUpgrade;
        Player.instance.canDrop = false;
        if (onUpgrade == false)
        {
            CancelItem(false);
        }
        else
        {
            ItemUsing(ItemType.ROTATE);
        }
    }
    public void ClearButton()
    {
        //Debug.Log("ClearButtonClicked");
        if (settingActive == true)
        {
            settingActive = false;
            Player.instance.canDrop = true;
            Debug.Log("Hide setting list");
            settingList.GetComponent<SettingList>().HideSettingList(() =>
            {
                settingList.SetActive(false);
            });
        }
    }
    public void HammerItem(int i)
    {
        bomb_lb.text = CheckTotalItem(i);
        EndlessLevel.Instance.UsingHammer();
    }
    public void ShakeItem(int i)
    {
        upgrade_lb.text = CheckTotalItem(i);
        EndlessLevel.Instance.UsingShake();
    }
    public void ChangeItem(int i)
    {
        tomato_lb.text = CheckTotalItem(i);
        EndlessLevel.Instance.UsingChange();
    }
    public void SettingButton()
    {
        PauseButton();
        DialogManager.Instance.ShowDialog(DialogIndex.SettingDialog);
    }
    public void SkinButton()
    {
        PauseButton();
        //DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);

    }
    public void RateButton()
    {
        PauseButton();
        ZenSDK.instance.Rate();
        //DialogManager.Instance.ShowDialog(DialogIndex.RateDialog);
    }
    public void RankButton()
    {
        PauseButton();
        //DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);
        //DialogManager.Instance.ShowDialog(DialogIndex.LeaderBoardDialog);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle the click event here
        Debug.Log("UI Object Clicked!");
    }
}
