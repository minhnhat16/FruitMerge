using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;


public class EndlessLevel : MonoBehaviour
{
    public static EndlessLevel Instance;
    [SerializeField] private int spriteType = 1;
    public int level;
    public int randomValue;
    public int score;
    public List<int> intQueue = new(5);
    [SerializeField] int life;
    [SerializeField] private float shakeItensity= 5f;
    [SerializeField] private float shakeTimer = 5f;
    [SerializeField]
    private bool isBomb = false;
    [SerializeField]
    private bool isUpgrade = false;
    [SerializeField] private List<CircleObject> _circles;
    public CircleObject main;
    [HideInInspector]
    public List<CircleObject> _Circles { get { return _circles; } }
    [HideInInspector]
    public bool IsBomb { get { return isBomb; } }
    [HideInInspector]
    public bool IsUpgrade { get { return isUpgrade; } }
    [HideInInspector]
    public int SpriteType { get { return spriteType; } set { spriteType = value; } }
    [HideInInspector]
    public int Life { get { return life; } }
    [HideInInspector]
    public int SetLife { set {  life = value; } }

    [HideInInspector]
    public UnityEvent<bool> onTarget = new UnityEvent<bool>();
    private void OnEnable()
    {
        onTarget.AddListener(TargetCircle);
    }
    private void OnDisable()
    {
        onTarget.RemoveAllListeners();
    }
    public void AddCircle(CircleObject item)
    {
        _Circles.Add(item);
    }
    public void RemoveCircle(CircleObject item)
    {
        var find = _Circles.Find(c => c == item);
            _circles.Remove(find);
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        life = 1;
        _circles = new List<CircleObject>();
        spriteType = DataAPIController.instance.GetCurrentFruitSkin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    void TargetCircle(bool isOnTarget)
    {
        if (isBomb == false)
        {
            DisableTargetCircles();
        }
        else
        {
            EnableTargetCircles();
        }
    }
    public void LoadLevel(Action callback)
    {
        level = 0;
        RandomCircle(); 
        callback?.Invoke();
    }
    public void RandomCircle()
    {
        while (intQueue.Count < 6)
        {

            if (intQueue.Count == 5)
            {

                StartCoroutine(SpawnFirstCircle());
            }
            if (level > 0 && level <= 3)
            {
                randomValue = GetRandomPositiveValue(level);
                intQueue.Add(randomValue);
            }
            else if (level > 3 && level <= 8)
            {
                randomValue = GetRandomPositiveValue(level - 2);
                intQueue.Add(randomValue);
            }
            else if (level > 8)
            {
                randomValue = GetRandomPositiveValue(level / 2);
                intQueue.Add(randomValue);
            }

        }

    }
    public int FirstInQueue()
    {
        return intQueue[0];
    }
    public IEnumerator SpawnFirstCircle()
    {
        //yield return new WaitForSeconds(spawnCooldown);
        yield return new WaitUntil(() => main == null);
        yield return new WaitUntil(() => IngameController.instance.isGameOver == false);
        int first = FirstInQueue();
        main = SpawnCircle(first);
        main.GotoState(main.Spawn);
        //Debug.Log("POS IN SPAWN PLAYER" + Player.instance.Pos);
    }
    public CircleObject SpawnCircle(int i)
    {
        switch (i)
        {
            case 1:
                //Debug.Log("IN CASE 1");
                CircleObject no1 = CirclePool.instance.pool.SpawnNonGravityNext();
                no1.SetTypeID(i);
                return no1;
            case 2:
                //Debug.Log("IN CASE 2");
                CircleObject no2 = CirclePool.instance.pool.SpawnNonGravityNext();
                no2.SetTypeID(i);
                return no2;
            case 3:
                //Debug.Log("IN CASE 3");
                CircleObject no3 = CirclePool.instance.pool.SpawnNonGravityNext();
                no3.SetTypeID(i);
                return no3;
            case 4:
                //Debug.Log("IN CASE 4");
                CircleObject no4 = CirclePool.instance.pool.SpawnNonGravityNext();
                no4.SetTypeID(i);
                return no4;
            case 5:
                //Debug.Log("IN CASE 4");
                CircleObject no5 = CirclePool.instance.pool.SpawnNonGravityNext();
                no5.SetTypeID(i);
                return no5;
            case 6:
                //Debug.Log("IN CASE 4");
                CircleObject no6 = CirclePool.instance.pool.SpawnNonGravityNext();
                no6.SetTypeID(i);
                return no6;
            default:
                return null;
        }
    }
    public void FindLargestType(int typeID)
    {
        if (typeID > level)
        {
            level = typeID;
            //Debug.Log("Updated type " + level);
        }
    }

    int GetRandomPositiveValue(int max)
    {
        double randStdNormal;
        int result;

        do
        {
            double u1 = 1.0 - Random.value;
            double u2 = 1.0 - Random.value;
            randStdNormal = Mathf.Sqrt((float)(-2.0 * Mathf.Log((float)u1))) * Mathf.Sin((float)(2.0 * Mathf.PI * u2));
            result = (int)Mathf.Round((float)(level * randStdNormal));
        } while (result <= 0 || result > max);

        return result;
    }
    public void UsingChange()
    {
    }
    public void DespawnMainCircle()
    {
        main.gameObject.SetActive(false);
    }
    public void SetActiveMainCircle(bool activate)
    {
        if (main == null) return;
        Debug.Log($"SetActiveMainCircle {activate}");
        main.gameObject.SetActive(activate);

    }
    public void UsingHammer()
    {
        Player.instance.canDrop = false;
        isBomb = true;
        onTarget?.Invoke(IsBomb);
    }
    public void UsingShake()
    {
        Player.instance.canDrop = false;
        isUpgrade = true;
        AddForceForCircle();
        WallScript.Instance.SetTopWallActive(false);
        CinemachineShake.instance.ShakeCamera(shakeItensity, shakeTimer);
        ShakeCouroutine();
    }
    public void ShakeCouroutine()
    {
        StartCoroutine(AfterUsingShake());
    }
    public IEnumerator AfterUsingShake()
    {
        var c = _circles.First();
        yield return new WaitForSeconds(c.ShakeDuration *0.75f);
        //yield return new WaitUntil(() => _circles.All(circle => circle.GetCurrentState() == "GroundedState"));
        Player.instance.canDrop = true;
        IngameController.instance.CancelItem();
        WallScript.Instance.SetTopWallActive(true);
    }
    public void AfterUsingBombItem()
    {
        Player.instance.canDrop = true;
        EnableTargetCircles();
        isBomb = false;
        IngameController.instance.CancelItem();
        onTarget?.Invoke(isBomb);
    }
    public void AfterUpgradeItem()
    {
        Player.instance.canDrop = true;
        isUpgrade = false;
        IngameController.instance.CancelItem();
        DisableTargetCircles();
    }
    public void EnableTargetCircles()
    {

        if (_circles == null ) return;
        foreach(var c in _circles)
        {
            c.EnableTarget();
        }
    }
    public void DisableTargetCircles()
    {
        //Debug.Log("DisableTargetCircles");
        if (_circles == null ) return;
        for (int i = 0; i < _circles.Count; i++)
        {
            _circles[i].DisableTarget();
        }
    }
    public void AddForceForCircle()
    {

        for (int i = 0; i  < _circles.Count; i ++)
        {
           
            if (_circles[i].gameObject.activeSelf)
            {
                _circles[i].ApplyForceOverTime();
            }
            else return;
        }
    }
    public void FreezeCircleDead()
    {
        foreach (var c in _circles)
        {
            c.GotoState(c.Dead);
        }
    }
    public void FreezeCircleRev()
    {
        foreach (var c in _circles)
        {
            c.SetRigidBodyToFreeze();
        }
    }
    public void UnfreezeCircles()
    {
        foreach(var c in _circles)
        {
            c.SetRigidBodyToNone();
        }
    }
    public void Clear()
    {
        level = 1;
        score = 0;
        CirclePool.instance.pool.DeSpawnAll();
        _circles.Clear();
        intQueue.Clear();   
    }
}
