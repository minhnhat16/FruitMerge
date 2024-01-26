using System;
using System.Collections;
using UnityEngine;

public class ConfigFileManager : MonoBehaviour
{
    public static ConfigFileManager Instance;

    [Header("CSV configs")]
    [SerializeField] private CircleTypeConfig circleConfig;
    [SerializeField] private PriceConfig priceConfig;
    [SerializeField] private ShopConfig shopConfig;
    [SerializeField] private ItemConfig itemConfig;

    [Header("Factory")]
    [SerializeField] private SoundFactory soundFactory;


    public CircleTypeConfig CircleConfig { get => circleConfig; }
    public PriceConfig PriceConfig { get => priceConfig; }
    public ShopConfig ShopConfig { get => shopConfig; }
    public ItemConfig ItemConfig { get => itemConfig; }

    public SoundFactory SoundFactory { get => soundFactory; }
    private void Awake()
    {
        Instance = this;
    }

    public void Init(Action callback)
    {
        Debug.Log("(BOOT) // INIT CONFIG");
        StartCoroutine(WaitInit(callback));
    }

    IEnumerator WaitInit(Action callback)
    {
        circleConfig = Resources.Load("Config/CircleTypeConfig", typeof(ScriptableObject)) as CircleTypeConfig;
        yield return new WaitUntil(() => circleConfig != null);
        priceConfig = Resources.Load("Config/PriceConfig", typeof(ScriptableObject)) as PriceConfig;
        yield return new WaitUntil(() => priceConfig != null);
        shopConfig = Resources.Load("Config/ShopConfig", typeof(ScriptableObject)) as ShopConfig;
        yield return new WaitUntil(() => priceConfig != null);
        itemConfig = Resources.Load("Config/ItemConfig", typeof(ScriptableObject)) as ItemConfig;
        yield return new WaitUntil(() => itemConfig != null);
        soundFactory = Resources.Load("Factory/SoundFactory", typeof(ScriptableObject)) as SoundFactory;
        SoundManager.Instance.Init();
        Debug.Log("(BOOT) // INIT CONFIG DONE");
        yield return new WaitUntil(() => soundFactory != null);
        yield return null;
        callback?.Invoke();
    }
}
