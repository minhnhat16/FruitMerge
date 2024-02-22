using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    [SerializeField] public bool isDisable;
    [SerializeField] private bool isOwned;
    [SerializeField] private int skinID;
    [SerializeField] private int price;
    public List<Image> fruitImages;
    public TextMeshProUGUI skinName_lb;
    public Image disableMask;
    public Image disOnwed;
    public Image equipedBG;
    public Image unquipedBG;
    public ConfirmButton confirmBtnType;
    public int SkinID { get => skinID; set => skinID = value; }
    public bool IsOwned { get => isOwned; set => isOwned = value; }
    public int Price { get => price; set => price = value; }
    public UnityEvent<bool> onClickAction = new UnityEvent<bool>();
    public UnityEvent<SkinItem> onEquipAction = new UnityEvent<SkinItem>();
    public void OnEnable()
    {
        onClickAction = confirmBtnType.onClickAction;
        onClickAction.AddListener(ButtonEvent);
    }
    public void InitSkin(int skinType/*,int price*/,bool isOwned, bool isDisable)
    {
        this.SkinID = skinType;
        this.isOwned = isOwned;
        this.isDisable = isDisable;
        this.price = price;
        CheckSkinIsObtain(isOwned);
    }
    public void CheckSkinIsObtain(bool isObtain)
    {
        if (isObtain && !isDisable)
        {
            SetItemEquiped();
            //ADD BUY OR EQUIPCONDITION
        }
        else if (isObtain && isDisable)
        {
            SetItemUnquiped();
        }
        else
        {
            SetItemBuy();
        }
    }
    public void SetItemEquiped()
    {
        disableMask.gameObject.SetActive(false);
        equipedBG.gameObject.SetActive(true);
        confirmBtnType.SwitchButtonType(ButtonType.Equiped);

    }
    public void SetItemUnquiped()
    {
        disableMask.gameObject.SetActive(false);
        unquipedBG.gameObject.SetActive(true);
        equipedBG.gameObject.SetActive(false);
        confirmBtnType.SwitchButtonType(ButtonType.Unquiped);

    }
    public void SetItemBuy()
    {
        disableMask.gameObject.SetActive(true);
        disOnwed.gameObject.SetActive(true);
        confirmBtnType.SwitchButtonType(ButtonType.Buy);
        confirmBtnType.UpdatePriceLb(price.ToString());
    }
    public void ButtonEvent(bool isClicked)
    {
        if (isClicked)
        {
            switch (confirmBtnType.Btntype)
            {
                case ButtonType.Ads:
                    Debug.Log("WATCH ADS TO GET NEW SKIN");
                    var param = new BuyConfirmDialogParam();
                    DialogManager.Instance.ShowDialog(DialogIndex.BuyConfirmDialog, null);
                    return;
                case ButtonType.Equiped:
                    Debug.Log("SKIN IS EQUIPPING");
                    //MAKE AN ACTION ON WARDROBE VIEW TO INVOKE THE ANIM FLOATING TEXT "EQUIPED SKIN"
                    return;
                case ButtonType.Unquiped: //SWITCH CURRENT SKIN FROM ANOTHER TO THIS
                    Debug.Log("SKIN EQUIPPED");
                    SetItemEquiped();
                    onEquipAction.Invoke(this);
                    return;
                case ButtonType.Buy:
                    Debug.Log("TRY TO BUY WITH AN AMOUNT OF GOLD");
                    DialogManager.Instance.ShowDialog(DialogIndex.BuyConfirmDialog, null);
                    return;
                default:
                    return;
            }
        }
    }
}
public class SkinViewItemAction
{
    public Action onItemSelect;
    public Action onItemEquip;
}
