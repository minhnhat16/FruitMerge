using NaughtyAttributes.Test;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    [SerializeField] private ButtonType btnType;
    [SerializeField] private string btnText;
    [SerializeField] List<Image> _typesImage;

    UnityEvent<bool> onClickAction = new UnityEvent<bool>();
    private void OnEnable()
    {
        onClickAction.AddListener(OnClickButton);
    }
    private void Start()
    {
        
    }
    public void OnClickButton( bool isClicked)
    {
        switch (btnType)
        {
            case ButtonType.Ads:
                Debug.Log("WATCH ADS TO GET NEW SKIN");
                break;  
            case ButtonType.Equiped:
                Debug.Log("SKIN IS EQUIPPING");
                break;
            case ButtonType.Unquiped: //SWITCH CURRENT SKIN FROM ANOTHER TO THIS
                Debug.Log("SKIN EQUIPPED");
                break;
            case ButtonType.Buy:
                Debug.Log("TRY TO BUY WITH AN AMOUNT OF GOLD");
                var param = new BuyConfirmDialogParam();
                break;
            default:
                break;
        }
    }
    public void OnClickAction()
    {

    }
    public void SwitchButtonType(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.Ads:
                //Ads type on 
                btnType = type;
                Debug.Log(type.ToString() + " int " + btnType);
                EnableButtonImage(type);
                break;
            case ButtonType.Buy:
                //Buy type on 
                btnType = type;
                EnableButtonImage(type);
                break;

            case ButtonType.Equiped:
                //Equiped type on 
                btnType = type;
                Debug.Log(type.ToString() + " int " + btnType);
                EnableButtonImage(type);
                break;

            case ButtonType.Unquiped:
                btnType = type;
                EnableButtonImage(type);
                break;
            //unEquiped type on 
            default:
                //disable button
                break;
        }
    }
    public void DisableAllButton()
    {
        foreach(var item in _typesImage)
        {
           item.gameObject.SetActive(false);

        }
    }
    public void EnableButtonImage(ButtonType type)
    {
        var item = _typesImage[(int)type];
        item.gameObject.SetActive(true);
    }
}

public enum ButtonType
{
    None = 0,
    Equiped =1,
    Unquiped =2,
    Ads = 3,
    Buy =4,
}

