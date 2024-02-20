using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    [SerializeField] public bool isDisable;
    [SerializeField] private bool isOwned;
    [SerializeField] private int skinID;
    public List<Image> fruitImages;
    public TextMeshProUGUI skinName_lb;
    public Image disableMask;
    public Image disOnwed;

    public ConfirmButton confirmBtnType;

    public int SkinID { get => skinID; set => skinID = value; }
    public bool IsOwned { get => isOwned; set => isOwned = value; }
    public void OnEnable()
    {
        
    }
    public void InitSkin(int skinType,bool isOwned, bool isDisable)
    {
        this.SkinID = skinType;
        this.isOwned = isOwned;
        this.isDisable = isDisable;
        CheckSkinIsObtain(isOwned);
    }
    public void CheckSkinIsObtain(bool isObtain)
    {
        if (isObtain)
        {
            disableMask.gameObject.SetActive(false);
            confirmBtnType.SwitchButtonType(ButtonType.Equiped);
            //ADD BUY OR EQUIPCONDITION
        }
        else
        {
            disableMask.gameObject.SetActive(true);
            disOnwed.gameObject.SetActive(true);
            confirmBtnType.SwitchButtonType(ButtonType.Buy);
        }
    }
    public void SetOwnedImg()
    {
        disableMask.gameObject.SetActive(false);
        confirmBtnType.SwitchButtonType(ButtonType.UnEquiped);
    }
}
public class SkinViewItemAction
{
    public Action onItemSelect;
    public Action onItemEquip;
}
