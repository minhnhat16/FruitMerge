﻿using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class DailyItem : MonoBehaviour
{
    public List<GameObject> backgrounds = new List<GameObject>();
    public Image itemImg;
    public int intAmount;
    public int day;
    public string itemName;
    public TextMeshProUGUI day_lb;
    public TextMeshProUGUI Amount_lb;
    public IEDailyType currentType;
    public Button daily_btn;
    [HideInInspector] public UnityEvent<bool> onClickDailyItem = new();
    [HideInInspector] public UnityEvent<bool> onItemClaim = new();

    private void OnEnable()
    {
        var parent = FindObjectOfType<DailyRewardDialog>();
        if (parent != null)
        {
            onClickDailyItem = parent.onClickDailyItem;
            onItemClaim = parent.onClickClaim;
        }
    }
    private void OnDisable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init( IEDailyType type, int amount, int day,string spriteName,string itemName)
    {
        SwitchType(type);
        SetAmountLb(amount);
        SetDayLB(day);
        SetItemImg(spriteName);
        SetItemNameType(itemName);
    }
    public void SetItemImg(string spriteName)
    {
        //Debug.Log(itemName);
        itemImg.sprite = SpriteLibControl.Instance.GetSpriteByName(spriteName);
    }
    public void SetItemNameType(string itemName)
    {
        this.itemName = itemName;
    }
    public void SetDayLB(int day)
    {
        //Debug.Log($"Set day lb {day}");
        this.day = day;
        day_lb.text += $"Day {day}";
    }
    public void SetAmountLb(int amount)
    {
        //Debug.Log($"Set amount lb {amount}");
        intAmount = amount;
        Amount_lb.text = amount.ToString();
    }
    public virtual void SwitchType(IEDailyType type)
    {
        currentType = type;
        daily_btn.enabled = true;
        switch (type)
        {
            case IEDailyType.Available:
                backgrounds[0].SetActive(true);
                backgrounds[1].SetActive(false);
                backgrounds[2].SetActive(false);
                daily_btn.enabled = true;
                break;
            case IEDailyType.Unavailable:
                backgrounds[1].SetActive(true);
                //daily_btn.gameObject.SetActive(false);
                break;
            case IEDailyType.Claimed:
                backgrounds[1].SetActive(false) ;
                backgrounds[0].SetActive(false);
                backgrounds[2].SetActive(true);
                daily_btn.enabled = false;
                Amount_lb.gameObject.SetActive(false);
                itemImg.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void SwitchItemType(string name) {
        switch (name) {
            case "gold":
                DataAPIController.instance.AddGold(intAmount);
                break;
            case "shake":
                DataAPIController.instance.AddItemTotal("0",intAmount);
                break;
            case "change":
                DataAPIController.instance.AddItemTotal("1", intAmount);
                break;
            case "burst":
                DataAPIController.instance.AddItemTotal("2", intAmount);
                break;
            default: break;
        }
    }
    public void ItemClaim(bool isClaim)
    {
        if (isClaim)
        {
            SwitchType(IEDailyType.Claimed);
            DataAPIController.instance.SetDailyData(day.ToString(), currentType);
            //int next = day++;
            //DataAPIController.instance.SetDailyData(next.ToString(), IEDailyType.Available);
            SwitchItemType(itemName);
        }
    }
    public virtual void OnClickDailyItem()
    {
        Debug.Log("On Click Daily Item");
        if(currentType == IEDailyType.Available)
        {
         //var parent = DialogManager.Instance.dicDialog[DialogIndex.DailyRewardDialog].GetComponent<DailyRewardDialog>();
         //   parent.dailyGrid.currentDaily = this;
            onClickDailyItem?.Invoke(true);
        }
        else
        {
            onClickDailyItem?.Invoke(false);
        }
    }
}