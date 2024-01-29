using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTemplate : MonoBehaviour
{
    [SerializeField] private Image backGround;
    [SerializeField] private Image itemImg;
    [SerializeField] private int type;
    [SerializeField] private Text name_lb;
    [SerializeField] private Image ContainBox;
    [SerializeField] private int  totalItem;
    [SerializeField] private Text total_lb;
    [SerializeField] private int intCost;
    [SerializeField] private Text cost_lb;
    [SerializeField] private bool enable;

    public int Type { get => type; set => type = value; }
    public int IntCost { get => intCost; set => intCost = value; }
    public int TotalItem { get => totalItem; set => totalItem = value; }
    public Image ItemImg { get => itemImg; set => itemImg = value; }
    public Text Name_lb { get => name_lb; set => name_lb = value; }
    public Image ContainBox1 { get => ContainBox; set => ContainBox = value; }
    public Text Total_lb { get => total_lb; set => total_lb = value; }
    public Text Cost_lb { get => cost_lb; set => cost_lb = value; }
    public bool Enable { get => enable; set => enable = value; }
    
   private void Start()
    {
       cost_lb.text = intCost.ToString();
       total_lb.text = "x" + totalItem.ToString();
    }
    BuyConfirmDialogParam param = new BuyConfirmDialogParam();
    public void OnClickBuyButton()
    {
        Debug.Log("ONLICKBUYBUTTON");
        int goldHave = DataAPIController.instance.GetGold();
        int intCost = Convert.ToInt32(cost_lb.text);
        //Debug.Log("gold" + goldHave + " && int cosst" +intCost + " cost " + cost_lb.text);
       
        param.onConfirmAction = () =>
        {
            if(type >0 && type <= 3)
            {
                int newType = type - 1;
                DataAPIController.instance.MinusGold(intCost);
                DataAPIController.instance.AddItemTotal(newType.ToString(), totalItem);
                IngameController.instance.GoldChanged();
            }
        };
        if (enable == true && goldHave >= intCost)
        {
            param.amount_lb = total_lb.text;
            param.cost_lb = cost_lb.text;
            DialogManager.Instance.ShowDialog(DialogIndex.BuyConfirmDialog, param);
        }
    }
}
