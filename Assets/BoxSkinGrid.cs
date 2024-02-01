using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSkinGrid : MonoBehaviour
{
    [SerializeField] private List<BoxItem> _boxes;
    [SerializeField] private int sumAvailableSkin;
   private void Awake()
    {
    }
    private void Start()
    {
        InitiateBoxSkinItem();
    }
    private void OnEnable()
    {
        sumAvailableSkin = 15;
    }
    private void InitiateBoxSkinItem()
    {
        for (int i = 0; i < sumAvailableSkin; i++)
        {
            var skin = Instantiate((Resources.Load("Prefab/UIPrefab/BoxSkin", typeof(GameObject))), transform) as GameObject;
            if (skin == null)
            {
                Debug.LogError(" item == null");
            }
            else
            {
                _boxes.Add(skin.GetComponent<BoxItem>());
                SetupSkinItem();
            }
        }
    }
    private void SetupSkinItem()
    {

    }
}
