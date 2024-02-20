using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinTabButton : MonoBehaviour
{
    public GameObject tabOn;
    public GameObject tabOff;
    public Animator animator;
    [SerializeField]private GameObject skinGrid;

    public void StartTabOn()
    {
        tabOn.SetActive(true);
        tabOff.SetActive(false);
        //animator.Play("TabSkin");
    }
    public void OnClickTabOn()
    {
        tabOn.SetActive(true);
        tabOff.SetActive(false);
        //animator.Play("TabSkin");
        skinGrid.GetComponentInChildren<Scrollbar>().value = 1;
        skinGrid.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClickSFX);
    }
    public void OnTabOff()
    {
        skinGrid.SetActive(false);
        tabOn.SetActive(false);
        tabOff.SetActive(true);
    }

}
