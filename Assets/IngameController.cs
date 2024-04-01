using UnityEngine;
using UnityEngine.Events;

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
    public Camera LoseCam { get { return loseCamera; } }
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
    private void OnDisable()
    {
        setNextCircleEvent.RemoveAllListeners();
        setScoreEvent.RemoveAllListeners();
        tomatoItemEvent.RemoveAllListeners();
        bombItemEvent.RemoveAllListeners();
        upgradeItemEvent.RemoveAllListeners();
        cancleItemEvent.RemoveAllListeners();
        gameOverEvent.RemoveAllListeners();
    }
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    // Update is called once per frame
    public void SetUpIngame()
    {
        backGround.SetActive(true);
        SetUpWall();
        SetUpPlayer();
        SetUpLevel();
        ResetScore();
        SetUpWire();
        EndlessLevel.Instance.RandomCircle();
        FirstCircle();
        loseCamera.orthographicSize += GameManager.instance.UIRoot.rate;
        return;
    }
    public void AddScore(int score)
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
        if (wire == null)
        {
            wire = Instantiate(Resources.Load("Prefab/Line", typeof(GameObject)), transform) as GameObject;
        }
        else
        {
            wire.gameObject.SetActive(true);
        }
    }
    public void SetUpPlayer()
    {
        Debug.Log($"Setup player {player}");

        if (player == null)
        {
           var p = Instantiate(Resources.Load("Prefab/Player", typeof(GameObject)), transform) as GameObject;
            Debug.Log($"Instantiate(Resources.Load(\"Prefab/Player\", typeof(GameObject)), transform) as GameObject {p}");
            player = p;
            player.GetComponent<Player>().canDrop= false;
        }
        else
        {
            player.SetActive(true); 
            player.GetComponent<Player>().Reset();
        }
    }
    public void SetUpWall()
    {
        Debug.Log($"Setup wall {wall}"); 
        if (wall == null)
        {
            var w = Instantiate(Resources.Load("Prefab/Wall", typeof(GameObject)), transform) as GameObject;
            Debug.Log($"Instantiate(Resources.Load(\"Prefab/Wall\", typeof(GameObject)), transform) as GameObject {w}");
            wall = w;
        }
        else
        {
            wall.gameObject.SetActive(true);
            wall.GetComponent<WallScript>().TopWallCouroutine();
        }
  
    }
    public void SetUpLevel()
    {
        var l = GetComponentInChildren<EndlessLevel>();
        if (l == null)
        {
            Level = Instantiate(Level, transform);
        }
        else
        {
            l.Clear();
        }
    }
    public void DestroyWall()
    {
        if (wall != null)
        {
            Destroy(wall);
            wall = null; 
        }
    }
    public void DestroyPlayer()
    {
        if (player != null)
        {
            Destroy(player);
            player = null;
        }
    }
    public void DestroyLine()
    {
        if (wire != null)
        {
            wire = null;
            Destroy(wire);
        }
    }
    public void DestroyIngameObject()
    {
        DestroyLine();
        DestroyPlayer();
        DestroyWall();
    }
    public void SetIngameObjectActive(bool isActive)
    {
        if (!isActive)
        {
            Destroy(wall, 0.5f);
            Destroy(wire, 0.5f);
            Destroy(player, 0.5f);
        }
        //wall.SetActive(isActive);
        //player.SetActive(isActive);
        //wire.SetActive (isActive);
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
        if (EndlessLevel.Instance.intQueue.Count != 0)
        {
            firstID = EndlessLevel.Instance.intQueue[1];
            setNextCircleEvent?.Invoke(firstID--);
        }
        else if (EndlessLevel.Instance.intQueue.Count == 0)
        {
            firstID = 0;
            Debug.LogWarning($"setNextCircleEvent?.Invoke({firstID})");
            setNextCircleEvent?.Invoke(firstID);
        }
    }
    public void ChangeItem()
    {
        int tomato = DataAPIController.instance.GetItemTotal(ItemType.CHANGE.ToString());
        tomato -= 1;
        DataAPIController.instance.SetItemTotal(ItemType.CHANGE.ToString(), tomato);
        tomatoItemEvent?.Invoke(tomato);
    }
    public void BursItem()
    {
        int bomb = DataAPIController.instance.GetItemTotal(ItemType.HAMMER.ToString());
        bomb -= 1;
        DataAPIController.instance.SetItemTotal(ItemType.HAMMER.ToString(), bomb);
        bombItemEvent?.Invoke(bomb);
    }
    public void ShakeItem()
    {
        int upgrade = DataAPIController.instance.GetItemTotal(ItemType.ROTATE.ToString());
        upgrade -= 1;
        DataAPIController.instance.SetItemTotal(ItemType.ROTATE.ToString(), upgrade);
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
    public void SwitchLoseCamOnOff(bool isOn)
    {
        Debug.Log($"SwitchLoseCamOnOff {isOn}");
        loseCamera.gameObject.SetActive(isOn);
        Debug.Log($"lose camera {loseCamera.isActiveAndEnabled}");
    }
    public void GameOver()
    {
        isGameOver = !isGameOver;
        Debug.Log("GameOver" + isGameOver);
        player.gameObject.SetActive(false);
        gameOverEvent?.Invoke(isGameOver);
    }
}
