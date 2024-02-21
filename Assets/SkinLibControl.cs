using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLibControl : MonoBehaviour
{
    public static SkinLibControl Instance;

    [SerializeField]
    private List<int> fruitSkinId = new List<int>();
    [SerializeField]
    private List<string> playerSkins = new List<string>();
    [SerializeField]
    private List<int> boxSkinID = new List<int>();
    [SerializeField]
    private List<string> boxSkins = new List<string>();
    private Dictionary<int, string> fruitSkinDic = new Dictionary<int, string>();
    private Dictionary<int, string> boxSkinDic = new Dictionary<int, string>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < playerSkins.Count; i++)
        {
            fruitSkinDic.Add(fruitSkinId[i], playerSkins[i]);
        }

        for (int i = 0; i < boxSkins.Count; i++)
        {
            boxSkinDic.Add(boxSkinID[i], boxSkins[i]);
        }
    }

    public string GetPlayerSkinById(int id)
    {
        return fruitSkinDic[id];
    }

    public string GetDinoSkinById(int id)
    {
        return boxSkinDic[id];
    }
}
