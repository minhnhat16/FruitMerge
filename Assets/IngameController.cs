using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class IngameController : MonoBehaviour
{
    public static IngameController instance;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject Level;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject wire;
    [SerializeField] private Camera loseCamera;
    [SerializeField] private int firstID;
    [SerializeField] private int score;
    public bool isPause = false;
    public bool isGameOver = false;

    public int FirstID { get { return firstID; } }
    public int Score { get { return score; } }
    public GameObject Wall { get { return wall; } }

    //[SerializeField] private GameObject level;
    // Start is called before the first frame update
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
    [HideInInspector]
    public UnityEvent<bool> gameOverEvent = new UnityEvent<bool>();
    [HideInInspector]
    public UnityEvent<int> onGoldChanged = new UnityEvent<int>();
    private void OnEnable()
    {
        
    }
    private void Awake()
    {
        if(instance == null)  instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

  
    public void SetUpIngame()
    {
        backGround.SetActive(true);
        SetUpPlayer();
        SetUpWall();
        SetUpLevel();
        ResetScore();
        EndlessLevel.Instance.RandomCircle();
        return;
    }
   public void AddScore (int score)
    {
        this.score += score;
        setScoreEvent?.Invoke(this.score);
    }
    public void ResetScore()
    {
        score = 0;
        setScoreEvent?.Invoke(score);
    }
    public void SetUpWire()
    {

    }
    public void SetUpPlayer()
    {
        var p = FindObjectOfType<Player>();
        if(p == null)
        {
            p = Instantiate(player, transform).GetComponent<Player>();
            player = p.gameObject;
        }
        else
        {
            player.gameObject.SetActive(true);  
        }
    }
    public void SetUpWall()
    {
        var w = FindObjectOfType<WallScript>();
        if(w == null)
        {
            wall = Instantiate(wall,transform);
           
        }
        wall.gameObject.SetActive(true);
    }
    public void SetUpLevel()
    {
        var l = FindObjectOfType<EndlessLevel>();
        if (l == null)
        {
            Level = Instantiate(Level, transform);
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void FirstCircle()
    {
       if(EndlessLevel.Instance.intQueue.Count != 0)
        {
            firstID = EndlessLevel.Instance.intQueue[1];
            setNextCircleEvent?.Invoke(firstID);
        }
    }
    public void TomatoItem()
    {
        int tomato = DataAPIController.instance.GetItemTotal("0");
        tomato -= 1;
        DataAPIController.instance.SetItemTotal("0", tomato);
        tomatoItemEvent?.Invoke(tomato);
    }
    public void BombItem()
    {
        int bomb = DataAPIController.instance.GetItemTotal("1");
        bomb -= 1;
        DataAPIController.instance.SetItemTotal("1", bomb);
        bombItemEvent?.Invoke(bomb);
    }
    public void UpgradeItem()
    {
        int upgrade = DataAPIController.instance.GetItemTotal("2");
        upgrade -= 1;
        DataAPIController.instance.SetItemTotal("1", upgrade);
        upgradeItemEvent?.Invoke(upgrade);
    }
    public void GoldChanged()
    {
        int totalGold = DataAPIController.instance.GetGold();
        onGoldChanged?.Invoke(totalGold);
    }
    public void CancelItem()
    {
        cancleItemEvent?.Invoke(false); 
    }
    public void GameOver()
    {
        isGameOver = !isGameOver;
        Debug.Log("GameOver" + isGameOver);
        loseCamera.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        gameOverEvent?.Invoke(isGameOver);
    }
}
