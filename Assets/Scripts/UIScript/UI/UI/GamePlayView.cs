using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] private Text hammerAll_lb;
    [SerializeField] private Animator goldAnim;


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
        bombItemEvent.AddListener(ScoreChange);
        upgradeItemEvent = IngameController.instance.setScoreEvent;
        upgradeItemEvent.AddListener(ScoreChange);
    }
    private void OnDisable()
    {
        setScoreEvent.RemoveListener(ScoreChange);
        setNextCircleEvent.RemoveListener(NextCircleImage);
        tomatoItemEvent.AddListener(TomatoItem);
        //bombItemEvent.AddListener(ScoreChange);
        //upgradeItemEvent.AddListener(ScoreChange);

    }
    public override void OnStartShowView()
    {
        base.OnStartShowView();
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
    //private void HammerOneChange()
    //{
    //    int total = DataAPIController.instance.GetItemTotal("0");

    //    if (hammerOne_lb.text != total.ToString())
    //    {
    //        hammerOne_lb.text = total.ToString();
    //    };
    //}
    //private void HammerAllChange()
    //{
    //    int total = DataAPIController.instance.GetItemTotal("1");

    //    if (hammerAll_lb.text != total.ToString())
    //    {
    //        hammerAll_lb.text = total.ToString();
    //    };
    //}
    public void NextCircleImage(int id)
    {
        var name = EndlessLevel.Instance.GetSpriteName(id);
        var sprite = SpriteLibControl.Instance.GetSpriteByName(name);
        nextBlock.sprite = sprite;
    }
    public void OnClickBomb()
    {
      
    }
    public void OnClickTomato()
    {
        int total = DataAPIController.instance.GetItemTotal("0");
        if (total > 0)
        {
            IngameController.instance.TomatoItem();
        }
    }
    public void OnClickUpgrade()
    {

    }
    public void TomatoItem(int i)
    {
        tomato_lb.text = i.ToString();
    }
    //public void OnClickHammer()
    //{
    //    int total = DataAPIController.instance.GetItemTotal("0");
    //    if (total > 0)
    //    {
    //        bool onHammer = GridSystem.instance.isHammerOne = true;
    //        onHammer = !onHammer;
    //        if (!onHammer)
    //        {

    //            GridSystem.instance.CanShift = false;
    //            GridSystem.instance.HammerOnOneBlock();
    //        }
    //        else
    //        {
    //            GridSystem.instance.isHammerOne = false;
    //            GridSystem.instance.CanShift = true;
    //            GridSystem.instance.MoveState();
    //        }
    //    }
    //}
    //public void OnClickHammerAll()
    //{
    //    int total = DataAPIController.instance.GetItemTotal("1");
    //    if (total > 0)
    //    {
    //        bool onHammer = GridSystem.instance.isHammerAll = true;
    //        onHammer = !onHammer;
    //        if (!onHammer)
    //        {
    //            GridSystem.instance.CanShift = false;
    //            GridSystem.instance.HammerOnAllBlockSameValue();
    //        }
    //        else
    //        {
    //            GridSystem.instance.isHammerAll = false;
    //            GridSystem.instance.CanShift = true;
    //            GridSystem.instance.MoveState();
    //        }
    //    }
    //}
}
