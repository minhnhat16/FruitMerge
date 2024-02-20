using UnityEngine;
using UnityEngine.UI;

public class WardrobeView : BaseView
{
    [SerializeField] private BoxTabButton boxTab;
    [SerializeField] private SkinTabButton skinTab;
    [SerializeField]  private GameObject skinGrid;
    [SerializeField] private GameObject boxSkin;

    [SerializeField] private Image currentSkin;
    public override void OnStartShowView()
    {
        base.OnStartHideView();
        StartTabOn();
    }
    public void StartTabOn()
    {
        skinGrid.SetActive(true);
        skinTab.StartTabOn();
        boxTab.OnTabOff();
    }
    public void SelectBoxTab()
    {
        boxTab.OnClickTabOn();
        skinTab.OnTabOff();

    }
    public void SelectSkinTab()
    {
        skinTab.OnClickTabOn();
        boxTab.OnTabOff();
    }
}
