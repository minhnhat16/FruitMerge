using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSkinGrid : MonoBehaviour
{
    [SerializeField] private List<BoxItem> _boxes;
    [SerializeField] private int sumAvailableSkin;
    private void OnEnable()
    {
        sumAvailableSkin = 15;
        InitiateSkinItem();
    }
    private void InitiateSkinItem()
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
