using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTabButton : MonoBehaviour
{
    public GameObject tabOn;
    public GameObject tabOff;
    public Animator animator;
    [SerializeField]private SkinGrid skinGrid;

    public void StartTabOn()
    {
        tabOn.SetActive(true);
        tabOff.SetActive(false);
        animator.Play("TabOn");
    }
    public void OnClickTabOn()
    {
        tabOn.SetActive(true);
        tabOff.SetActive(false);
        animator.Play("TabOn");

        skinGrid.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX.UIClickSFX);
    }
    public void OnTabOff()
    {
        skinGrid.gameObject.SetActive(false);
        tabOn.SetActive(false);
        tabOff.SetActive(true);
    }

}
