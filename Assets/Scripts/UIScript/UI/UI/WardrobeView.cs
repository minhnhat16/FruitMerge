using UnityEngine;

public class WardrobeView : BaseView
{
    [SerializeField] private BoxTabButton boxTab;
    [SerializeField] private SkinTabButton skinTab;
    public override void OnStartShowView()
    {
        base.OnStartHideView();
        StartTabOn();
    }
    public void StartTabOn()
    {
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
