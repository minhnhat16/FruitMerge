using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockWorkTuto : MonoBehaviour
{

    [SerializeField] private List<Image> levelImgs = new();
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        int skinID = DataAPIController.instance.GetCurrentFruitSkin();
        foreach (Image level in levelImgs)
        {
            string name = SpriteLibControl.Instance.GetCircleSpriteName(skinID, i);
            level.sprite = SpriteLibControl.Instance.GetSpriteByName(name);
            i++;
        }
    }
  
}
