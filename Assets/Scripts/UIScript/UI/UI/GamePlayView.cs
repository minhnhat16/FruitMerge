using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePlayView : BaseView, IPointerClickHandler
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
        tomatoItemEvent.AddListener(TomatoItem);
        bombItemEvent = IngameController.instance.bombItemEvent;
        bombItemEvent.AddListener(BombItem);
        upgradeItemEvent = IngameController.instance.upgradeItemEvent;
        upgradeItemEvent.AddListener(UpgradeItem);
        cancleItemEvent = IngameController.instance.cancleItemEvent;
        cancleItemEvent.AddListener(CancelItem);

    }
    private void OnDisable()
    {
        setScoreEvent.RemoveListener(ScoreChange);
        setNextCircleEvent.RemoveListener(NextCircleImage);
        tomatoItemEvent.AddListener(TomatoItem);
        bombItemEvent.AddListener(BombItem);
        upgradeItemEvent.AddListener(UpgradeItem);
    }
    public override void OnStartShowView()
    {
        base.OnStartShowView();
        onTomato = false;
        onBomb = false;
        onUpgrade = false;
        //tomato_Btn.onValueChanged.AddListener(OnClickTomato);
        //bomb_Btn.onValueChanged.AddListener(OnClickBomb);
        //upgrade_Btn.onValueChanged.AddListener(OnClickUpgrade);
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
        DialogManager.Instance.ShowDialog(DialogIndex.PauseDialog, null, () =>
        {
            var player = FindObjectOfType<Player>();
            //player.canDrop = false;
            IngameController.instance.isPause = true;
        });
    }
    public void NextCircleImage(int id)
    {
        nextBlock.transform.DOScale(0.1f, 0);
        Tween tween = nextBlock.transform.DOScale(0.65f, 0.25f).SetEase(Ease.OutBounce);
        var name = EndlessLevel.Instance.GetSpriteName(id);
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
        else
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
        onTomato = false;
        onUpgrade = false;
        onBomb = false;
    }
    public void OnClickTomato()
    {
        onTomato = true;
        ItemUsing(0);
    }
    public void OnClickBomb()
    {
        onBomb = true;
        ItemUsing(1);
    }
    public void OnClickUpgrade()
    {
        onUpgrade = true;
        ItemUsing(2);
    }
    public void BombItem(int i)
    {
        bomb_lb.text = i.ToString();
        EndlessLevel.Instance.UsingBombItem();
    }
    public void UpgradeItem(int i)
    {
        upgrade_lb.text = i.ToString();
        EndlessLevel.Instance.UsingUpgradeItem();
    }
    public void TomatoItem(int i)
    {
        tomato_lb.text = i.ToString();
        EndlessLevel.Instance.UsingTomato();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the mouse is over the toggle
        if (eventData.pointerCurrentRaycast.gameObject == tomato_Btn.gameObject)
        {
            // Handle toggle click
            OnClickTomato();
        }
        else if (eventData.pointerCurrentRaycast.gameObject == bomb_Btn.gameObject)
        {
            // Handle toggle click
            OnClickBomb();
        }
        else if (eventData.pointerCurrentRaycast.gameObject == upgrade_lb.gameObject)
        {
            // Handle toggle click
            OnClickUpgrade();
        }
    }

}
