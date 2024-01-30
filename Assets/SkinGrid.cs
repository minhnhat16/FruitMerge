using System.Collections.Generic;
using UnityEngine;

public class SkinGrid : MonoBehaviour
{
    [SerializeField] private  List<SkinItem> _skins;
    [SerializeField] private int sumAvailableSkin ;
    private void OnEnable()
    {
        sumAvailableSkin = 6;
        InitiateSkinItem();
    }
    private void InitiateSkinItem()
    {
        //var ownSkin = DataAPIController.instance.GetAllFruitSkinOwned();
       for (int i = 0; i < sumAvailableSkin; i++)
        {
            var skin = Instantiate((Resources.Load("Prefab/UIPrefab/SkinItemPrefab", typeof(GameObject))), transform) as GameObject;
            if (skin == null)
            {
                Debug.LogError(" item == null");
            }
            else
            {
                _skins.Add(skin.GetComponent<SkinItem>());
                SetupSkinItem();
            }
        }
    }
    private void SetupSkinItem()
    {

    }
}
