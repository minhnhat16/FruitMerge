
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EndlessLevel : MonoBehaviour
{
    public static EndlessLevel Instance;
    public int level;
    public List<int> intQueue = new(5);
    public CircleObject main;
    public int randomValue;
    public int score;
    private float spawnCooldown = 0.1f;
    [SerializeField]private List<CircleObject> _circles ;
    [SerializeField] private List<CircleObject> _sortedTest;

    [HideInInspector]
    public  List<CircleObject> _Circles { get { return _circles; }}
    public void AddCircle(CircleObject item)
    {
        _Circles.Add(item);
    }
    public void RemoveCircle(CircleObject item)
    {
        _Circles.Find(item=> _circles.Remove(item));
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
        int first = FirstInQueue();
        main = SpawnCircle(first);
        main.GotoState(main.Spawn);
        //Debug.Log("POS " + Player.instance.Pos);
        main.transform.position = Player.instance.Pos;
        yield return new WaitForSeconds(spawnCooldown);
    }
    public CircleObject SpawnCircle(int i)
    {
        switch (i)
        {
            case 1:
                //Debug.Log("IN CASE 1");
                CircleObject no1 = CirclePool.instance.pool.SpawnNonGravity();
                no1.SetTypeID(i);
                return no1;
            case 2:
                //Debug.Log("IN CASE 2");
                CircleObject no2 = CirclePool.instance.pool.SpawnNonGravity();
                no2.SetTypeID(i);
                return no2;
            case 3:
                //Debug.Log("IN CASE 3");
                CircleObject no3 = CirclePool.instance.pool.SpawnNonGravity();
                no3.SetTypeID(i);
                return no3;
            case 4:
                //Debug.Log("IN CASE 4");
                CircleObject no4 = CirclePool.instance.pool.SpawnNonGravity();
                no4.SetTypeID(i);
                return no4;
            case 5:
                //Debug.Log("IN CASE 4");
                CircleObject no5 = CirclePool.instance.pool.SpawnNonGravity();
                no5.SetTypeID(i);
                return no5;
            case 6:
                //Debug.Log("IN CASE 4");
                CircleObject no6 = CirclePool.instance.pool.SpawnNonGravity();
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
        foreach( var c in sortedCircles)
        {
            RemoveCircle(c);
            c.RemoveCircle();
        }
    }
    public  List<CircleObject> CirclesBelow3(List<CircleObject> circles)
    {
        List<CircleObject> below3 = new();
        for(int i = 0; i < circles.Count; i++)
        {
            if (circles[i].TypeID <3 && circles[i].TypeID > 0) 
            {
                below3.Add(circles[i]);
            }
        }
        _sortedTest = below3;
        return below3;
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

    public void Clear()
    {
        level = 1;
        score = 0;
        CirclePool.instance.pool.DeSpawnAll();
        intQueue.Clear();
        RandomCircle();
    }
    public string GetSpriteName(int id)
    {
        switch (id)
        {
            case 1:
                return "blueberry_whiteStroke";
            case 2:
                return "cherry_whiteStroke";
            case 3:
                return "lemon_whiteStroke";
            case 4:
                return "mango_whiteStroke";
            case 5:
                return "orange_whiteStroke";
            case 6:
                return "apple_whiteStroke";
            case 7:
                return "peach_whiteStroke";
            case 8:
                return "coconut_whiteStroke";
            case 9:
                return "melon_whiteStroke";
            case 10:
                return "pineapple_whiteStroke";
            case 11:
                return "watermelon_whiteStroke";
            default:
                return null;
        }
    }
}
