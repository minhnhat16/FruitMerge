using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    [SerializeField] public bool isDisable;
    [SerializeField] private bool isObtain;
    [SerializeField] private int skinType;
    public List<Image> fruitImages;
    public Text skinName_lb;
    public Image disableMask;
    public List<Image> confirmBtnType;

    public void InitFruitSkin()
    {
        int i = 0;
        foreach (var image in fruitImages)
        {
          var skinName=  SpriteLibControl.Instance.GetSpriteName(skinType, i);
            image.sprite = SpriteLibControl.Instance.GetSpriteByName(skinName);
            i++;
        }
    }
    public void CheckSkinIsObtain()
    {

    }
}
