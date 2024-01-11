using DG.Tweening;
using System.Linq;
using Unity.VisualScripting;
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
        upgradeItemEvent = IngameController.instance.setScoreEvent;
        upgradeItemEvent.AddListener(ScoreChange);
    }
    private void OnDisable()
    {
        setScoreEvent.RemoveListener(ScoreChange);
        setNextCircleEvent.RemoveListener(NextCircleImage);
        tomatoItemEvent.AddListener(TomatoItem);
        bombItemEvent.AddListener(BombItem);
        //upgradeItemEvent.AddListener(ScoreChange);
    }
    public override void OnStartShowView()
    {
        base.OnStartShowView();
        if (tomato_Btn != null)
        {
            tomato_Btn.onClick.AddListener(OnClickTomato);
            bomb_Btn.onClick.AddListener(OnClickBomb);
        }
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
     
    }
    public void ShowGoldAnim(int gold)
    {
        _changeGold = gold;
        int calGold = _changeGold - _curGold;
        if (calGold == 0) return;

        _curGold = _changeGold;
        Debug.LogWarning("GOLD SHOW ANIM");
        goldAnim.gameObject.SetActive(true) ;
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
        tween.OnComplete(() => {
            tween?.Kill();
        });
    }
    public void OnClickBomb()
    {
        int total = DataAPIController.instance.GetItemTotal("1");
        if (total > 0)
        {
            Debug.Log("ON CLICK BOMB ");
            IngameController.instance.BombItem();
        }
    }
    public void OnClickTomato()
    {
        int total = DataAPIController.instance.GetItemTotal("0");
        if (total > 0)
        {
            Debug.Log("ON CLICK TOMATO ");
            IngameController.instance.TomatoItem();
        }
    }
    public void OnClickUpgrade()
    {

    }
    public void BombItem(int i)
    {
        bomb_lb.text = i.ToString();
        EndlessLevel.Instance.UsingBombItem();
    }
    public void TomatoItem(int i)
    {
        tomato_lb.text = i.ToString();
        EndlessLevel.Instance.UsingTomato();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the mouse is over the button and not interacting with the specified game object
        if (eventData.pointerCurrentRaycast.gameObject == tomato_Btn.gameObject /*&&
            eventData.pointerPress != GetComponent<Player>()*/)
        {
            // Handle button click
            OnClickTomato();
            OnClickBomb();
        }
    }
}
