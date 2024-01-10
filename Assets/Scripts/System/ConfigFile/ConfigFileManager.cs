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

    public CircleTypeConfig CircleConfig { get => circleConfig; }
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
        circleConfig = Resources.Load("Configs/CircleTypeConfig", typeof(ScriptableObject)) as CircleTypeConfig;
        yield return new WaitUntil(() => circleConfig != null);
        Debug.Log("(BOOT) // INIT CONFIG DONE");

        yield return null;
        callback?.Invoke();
    }
}
