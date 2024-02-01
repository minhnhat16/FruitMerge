using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxSkinGrid : MonoBehaviour
{
    [SerializeField] private List<BoxItem> _boxes;
    [SerializeField] private int sumAvailableSkin;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        sumAvailableSkin = 15;
        InitiateBoxSkinItem();
    }
    private void OnEnable()
    {
        ResetScroll();
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
    public void ResetScroll()
    {
        // Check if the ScrollRect is assigned
        if (scrollRect != null)
        {
            // Reset the scroll position to the top
            Debug.LogError("Reset ScrollReact");

            scrollRect.normalizedPosition = new Vector2(0f, 1f);
        }
        else
        {
            Debug.LogError("ScrollRect not assigned to the script.");
        }
    }
    private void SetupSkinItem()
    {

    }
}
