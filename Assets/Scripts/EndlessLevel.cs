using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessLevel : MonoBehaviour
{
    public static EndlessLevel Instance;
    [SerializeField] private int spriteType = 1;
    public int level;
    public List<int> intQueue = new(5);
    public CircleObject main;
    public int randomValue;
    public int score;
    [SerializeField] private float spawnCooldown = 0.1f;
    [SerializeField]
    private bool isBomb = false;
    [SerializeField]
    private bool isUpgrade = false;
    [SerializeField] private List<CircleObject> _circles;

    [HideInInspector]
    public List<CircleObject> _Circles { get { return _circles; } }
    public bool IsBomb { get { return isBomb; } }
    public bool IsUpgrade { get { return isUpgrade; } }
    public int SpriteType { get { return spriteType; } }
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
        _circles = new List<CircleObject>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadLevel(Action callback)
    {
        level = 1;
        Player.instance.ResetPos();
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
        if (IngameController.instance.isGameOver)yield return null;
        yield return new WaitForSeconds(spawnCooldown);
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
    public void UsingTomato()
    {
        var sortedCircles = _circles;
        //BubbleSortCircle(sortedCircles);
        sortedCircles.OrderBy(c => c.TypeID);
        sortedCircles = CirclesBelow3(sortedCircles);
        foreach (var c in sortedCircles)
        {
            if (c != main)
            {
                RemoveCircle(c);
                c.RemoveCircle();
            }

        }
    }
    public void DespawnMainCircle()
    {
        Debug.Log("Despawn Main Circle");
        _circles.Last().gameObject.SetActive(false);
        foreach (var c in _circles)
        {
            if (c.GetCurrentState() == "SpawnState")
            {
                Debug.Log($"{c.name} Despawn Main Circle");
                c.gameObject.SetActive(false);
            }
        }
    }
    public List<CircleObject> CirclesBelow3(List<CircleObject> circles)
    {
        List<CircleObject> below3 = new();
        for (int i = 0; i < circles.Count; i++)
        {
            if (circles[i].TypeID < 3 && circles[i].TypeID > 0)
            {
                below3.Add(circles[i]);
            }
        }
        return below3;
    }
    public void UsingBombItem()
    {
        Player.instance.canDrop = false;
        isBomb = true;
        EnableTargetCircles();
    }
    public void UsingUpgradeItem()
    {
        Player.instance.canDrop = false;
        isUpgrade = true;
        WallScript.Instance.ShakeWall();
    }

    public void AfterUsingBombItem()
    {
        Player.instance.canDrop = true;
        isBomb = false;
        IngameController.instance.CancelItem();
        DisableTargetCircles();
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
        //Debug.Log("EnableTargetCircles");

        if (_circles == null) return;
        for (int i = 0; i < _circles.Count - 1; i++)
        {
            _circles[i].EnableTarget();
        }

    }
    public void DisableTargetCircles()
    {
        //Debug.Log("DisableTargetCircles");
        if (_circles == null) return;
        for (int i = 0; i < _circles.Count - 1; i++)
        {
            _circles[i].DisableTarget();
        }
    }
    public static void BubbleSortCircle(List<CircleObject> circles)
    {
        int n = circles.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (circles[j].TypeID > circles[j + 1].TypeID)
                {
                    // Swap circles[j] and circles[j+1]
                    var temp = circles[j];
                    circles[j] = circles[j + 1];
                    circles[j + 1] = temp;
                }
            }
        }
    }
    public void FreezeCircle()
    {
        foreach (var c in _circles)
        {
            c.GotoState(c.Dead);
        }
    }
    public void Clear()
    {
        level = 1;
        score = 0;
        CirclePool.instance.pool.DeSpawnAll();
        intQueue.Clear();
    }

}
