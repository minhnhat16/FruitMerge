using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    [SerializeField] private ButtonType btnType;
    [SerializeField] private string btnText;
    [SerializeField] List<Image> _typesImage;
    private void Start()
    {
        
    }
    public void OnClickButton()
    {
        switch (btnType)
        {
            case ButtonType.Ads:
                Debug.Log("WATCH ADS TO GET NEW SKIN");
                OnBuyAction();
                break;  
            case ButtonType.Equiped:
                Debug.Log("SKIN EQUIPPED");
                break;
            case ButtonType.Buy:
                Debug.Log("TRY TO BUY WITH AN AMOUNT OF GOLD");
                var param = new BuyConfirmDialogParam();
                OnBuyAction();
                break;
            default:
                break;
        }
    }
    public void OnBuyAction()
    {
        DialogManager.Instance.ShowDialog(DialogIndex.BuyConfirmDialog);

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
                //EnableButtonImage(btnType);
                break;

            case ButtonType.Buy:
                //Buy type on 

                btnType = type;
                EnableButtonImage(type);

                //EnableButtonImage(btnType);
                break;

            case ButtonType.Equiped:
                //Equiped type on 
                btnType = type;
                EnableButtonImage(type);

                //EnableButtonImage(btnType);
                break;

            case ButtonType.UnEquiped:
                btnType = type;
                EnableButtonImage(type);

                //EnableButtonImage(btnType);
                break;

            //unEquiped type on 
            default:
                //disable button
                break;
        }
    }
    public void EnableButtonImage(ButtonType type)
    {
      for(int i = 0; i < _typesImage.Count; i++)
        {
            if (i != ((int)type))
            {
                _typesImage[i].gameObject.SetActive(false);
            }
            else
            {
                _typesImage[i].gameObject.SetActive(true);
            }
        }
    }
}

public enum ButtonType
{
    None = 0,
    Ads = 2,
    Buy =3 ,
    Equiped =4,
    UnEquiped =5,
}

