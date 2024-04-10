using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClockWorkTuto : MonoBehaviour
{

    [SerializeField] private List<Image> levelImgs = new();
    private int skinID;
    UnityEvent<int> skinChanged = new UnityEvent<int>();
    private void OnEnable()
    {
        skinChanged = IngameController.instance.skinChanged;
        skinChanged.AddListener(ChangeSkinByIndex);
        skinID = DataAPIController.instance.GetCurrentFruitSkin();
        ChangeSkinByIndex(skinID);
    }
    // Start is called before the first frame update
    private void OnDisable()
    {
        skinChanged.RemoveAllListeners();
    }
    void Start()
    {
        skinID = DataAPIController.instance.GetCurrentFruitSkin();
        ChangeSkinByIndex(skinID);
    }
    void ChangeSkinByIndex(int skinIndex)
    {
        Debug.Log("ChangeSkinByIndex");
        int i = 0;
        foreach (Image level in levelImgs)
        {
            string name = SpriteLibControl.Instance.GetCircleSpriteName(skinID, i);
            level.sprite = SpriteLibControl.Instance.GetSpriteByName(name);
            i++;
        }
    }
}
