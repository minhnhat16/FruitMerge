using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePlayView : BaseView
{
    [HideInInspector] GamePlayAnim anim;
    [SerializeField] Text score_lb;
    [SerializeField] private int _changeGold;
    [SerializeField] private int _curGold;
    [SerializeField] private Image nextBlock;
    [SerializeField] private Text nextValue;
    [SerializeField] public Text gold_lb;
    [SerializeField] private Text tomato_lb;
    [SerializeField] private Text bomb_lb;
    [SerializeField] private Text upgrade_lb;
    [SerializeField] private Text hammerAll_lb;
    [SerializeField] private Animator goldAnim;
    [SerializeField] private GameObject settingList;

    [SerializeField] Button tomato_Btn;
    [SerializeField] Button bomb_Btn;
    [SerializeField] Button upgrade_Btn;
    [SerializeField] bool onTomato;
    [SerializeField] bool onBomb;
    [SerializeField] bool onUpgrade;
    [SerializeField] List<Image> exit_img;
    [HideInInspector]
    public UnityEvent<int> setNextCircleEvent = new UnityEvent<int>();
    [HideInInspector]
    public UnityEvent<int> setScoreEvent = new UnityEvent<int>();
    [HideInInspector]
    public UnityEvent<int> tomatoItemEvent = new UnityEvent<int>();
    [HideInInspector]
    public UnityEvent<int> bombItemEvent = new UnityEvent<int>();
    [HideInInspector]
    public UnityEvent<int> upgradeItemEvent = new UnityEvent<int>();
    [HideInInspector]
    public UnityEvent<bool> cancleItemEvent = new UnityEvent<bool>();
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

        tomato_lb.text = DataAPIController.instance.GetItemTotal("0").ToString();
        bomb_lb.text = DataAPIController.instance.GetItemTotal("1").ToString();
        upgrade_lb.text = DataAPIController.instance.GetItemTotal("2").ToString();
        
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
        if (onBomb && onTomato&& onUpgrade)
        {
            foreach (var e in exit_img)
            {
                e.gameObject.SetActive(true);
            }
        }
        else {
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
        score_lb.text = score.ToString();
    }
    public void PauseButton()
    {
        if(settingList.activeSelf == true)
        {
            Debug.Log("pause button list ON");
            settingList.GetComponent<SettingList>().HideSettingList(() =>
            {
                settingList.SetActive(false);

            });
        }
        else
        {
            settingList.SetActive(true);
            settingList.GetComponent<SettingList>().ShowSettingList(null);

        }
    }
    public void NextCircleImage(int id)
    {
        nextBlock.transform.DOScale(0.1f, 0);
        Tween tween = nextBlock.transform.DOScale(0.65f, 0.25f).SetEase(Ease.OutBounce);
        
        var name = SpriteLibControl.Instance.GetSpriteName(EndlessLevel.Instance.SpriteType, id);
        var sprite = SpriteLibControl.Instance.GetSpriteByName(name);
        nextBlock.sprite = sprite;
        tween.OnComplete(() =>
        {
            tween?.Kill();
        });
    }
    public void ItemUsing(int type )
    {
        int total = DataAPIController.instance.GetItemTotal(type.ToString());
        if (!onTomato && !onUpgrade && !onBomb)
        {
            CancelItem(false) ;
        }
        else if(onTomato && onUpgrade && onBomb) {  }
        {
            ItemConfirmParam param = new();
            param.type = type;
            param.name = "AAA";

            if (total > 0 && EndlessLevel.Instance._Circles.Count != 0)
            {
                DialogManager.Instance.ShowDialog(DialogIndex.ItemConfirmDialog, param, () =>
                {
                    onTomato = true;
                    onUpgrade = true;
                    onBomb = true;
                });
            }
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
    public void O()
    {
        onTomato = !onTomato;
        Player.instance.canDrop = false;
        if(onTomato == false)
        {
            CancelItem(false);
        }
        else
        {
            ItemUsing(0);
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
        ItemUsing(1);
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
            ItemUsing(2);
        }
    }
    public void HammerItem(int i)
    {
        bomb_lb.text = i.ToString();
        EndlessLevel.Instance.UsingHammer();
    }
    public void ShakeItem(int i)
    {
        upgrade_lb.text = i.ToString();
        EndlessLevel.Instance.UsingShake();
    }
    public void ChangeItem(int i)
    {
        tomato_lb.text = i.ToString();
        EndlessLevel.Instance.UsingChange();
    }
    public void SettingButton()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.SettingDialog);
    }
    public void SkinButton()
    {
        //DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);

    }
    public void RateButton()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.RateDialog);
    }
    public void RankButton()
    {
        //DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);
        //DialogManager.Instance.ShowDialog(DialogIndex.LeaderBoardDialog);

    }
}
