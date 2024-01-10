using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyDialog : BaseDialog
{
    //[SerializeField] private BaseView shopView;
    //[SerializeField] private Text amount_lb;
    //[SerializeField] private Text bonus_lb;
    //[SerializeField] private GameObject success_cv;
    //[SerializeField] private bool successBuy;
    //public override void Setup(DialogParam dialogParam)
    //{

    //    shopView = ViewManager.Instance.currentView;
    //    base.Setup(dialogParam);
    //    if(dialogParam != null)
    //    {
    //        BuyConfirmDialogParam param = (BuyConfirmDialogParam)dialogParam;
    //        amount_lb.text = param.amount_lb;
    //        bonus_lb.text = param.bonus_lb;
    //    }
    //}
    //public override void OnStartShowDialog()
    //{
    //}
  
    //public void CancelButton()
    //{
    //    Debug.Log("CANCEL");    
    //    DialogManager.Instance.HideDialog(DialogIndex.BuyConfirmDialog);

    //}
    //public void ConfirmButton()
    //{
    //    Debug.Log("CONFIRM");
    //    int total_gold = DataAPIController.instance.GetGold();
    //    total_gold += Convert.ToInt32(amount_lb.text);
    //    DataAPIController.instance.SaveGold(total_gold);
    //    shopView.GetComponent<ShopView>().OnUpdateGold();
    //    success_cv.SetActive(true);
    //    successBuy = true;
    //    StartCoroutine(BuySuccess());
    //}
    //public IEnumerator BuySuccess()
    //{
    //    if (successBuy == true)
    //    {
    //        Debug.Log("BUY SUCCESS");
    //        //successBuy = false;
    //        yield return new WaitForSeconds(2f);
    //        HideCurennt();
    //    }
    //}

    //public void HideCurennt()
    //{
    //    successBuy = false;
    //    success_cv.SetActive(false);
    //    DialogManager.Instance.HideDialog(DialogIndex.BuyConfirmDialog);

    //}
}
