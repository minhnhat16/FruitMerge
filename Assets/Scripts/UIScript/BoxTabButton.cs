using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxTabButton : MonoBehaviour
{
    public GameObject tabOn;
    public GameObject tabOff;
    public Animator animator;
    public GameObject boxGrid;
 
    public void OnClickTabOn()
    {
        tabOn.SetActive(true);
        tabOff.SetActive(false);
        //animator.Play("TabSkin");
        boxGrid.GetComponentInChildren<ScrollRect>().verticalScrollbar.value = 1;
        boxGrid.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClickSFX);
    }
    public void OnTabOff()
    {
        boxGrid.SetActive(false);
        tabOn.SetActive(false);
        tabOff.SetActive(true);
    }
}
