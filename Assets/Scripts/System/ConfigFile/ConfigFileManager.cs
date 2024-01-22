using System;
using System.Collections;
using UnityEngine;

public class ConfigFileManager : MonoBehaviour
{
    public static ConfigFileManager Instance;

    //[Header("Factory configs")]
    //[SerializeField] private MapObjFactory objFactory;
    //[SerializeField] private SoundFactory soundFactory;

    [Header("CSV configs")]
    [SerializeField] private CircleTypeConfig circleConfig;

    [Header("Factory")]
    [SerializeField] private SoundFactory soundFactory;

    public CircleTypeConfig CircleConfig { get => circleConfig; }
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
        circleConfig = Resources.Load("Config/CircleTypeConfig_1", typeof(ScriptableObject)) as CircleTypeConfig;
        yield return new WaitUntil(() => circleConfig != null);
        soundFactory = Resources.Load("Factory/SoundFactory", typeof(ScriptableObject)) as SoundFactory;
        Debug.Log("(BOOT) // INIT CONFIG DONE");
        SoundManager.Instance.Init();
        yield return new WaitUntil(() => soundFactory != null);

        yield return null;
        callback?.Invoke();
    }
}
