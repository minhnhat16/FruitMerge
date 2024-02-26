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
    public TextMeshProUGUI day_lb;
    public TextMeshProUGUI Amount_lb;
    public IEDailyType currentType;
    public Button daily_btn;
    [HideInInspector] public UnityEvent<bool> onClickDailyItem = new();
    private void OnEnable()
    {
        var parent = DialogManager.Instance.dicDialog[DialogIndex.DailyRewardDialog].GetComponent<DailyRewardDialog>();
        onClickDailyItem  = parent.onClickDailyItem;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(IEDailyType type, int amount, int day,string itemName)
    {
        SwitchType(type);
        SetAmountLb(amount);
        SetDayLB(day);
        SetItemImg(itemName);
    }
    public void SetItemImg(string itemName)
    {
        Debug.Log(itemName);
        itemImg.sprite = SpriteLibControl.Instance.GetSpriteByName(itemName);
    }

    public void SetDayLB(int day)
    {
        Debug.Log($"Set day lb {day}");
        day_lb.text += $"Day {day}";
    }
    public void SetAmountLb(int amount)
    {
        Debug.Log($"Set amount lb {amount}");
        intAmount = amount;
        Amount_lb.text = amount.ToString();

    }
    public void SwitchType(IEDailyType type)
    {
        currentType = type;
        switch (type)
        {
            case IEDailyType.Available:
                backgrounds[0].SetActive(true);
                daily_btn.enabled = true;
                break;
            case IEDailyType.Unavailable:
                backgrounds[1].SetActive(true);
                daily_btn.enabled = false;

                //daily_btn.gameObject.SetActive(false);
                break;
            case IEDailyType.Claimed:
                backgrounds[2].SetActive(true);
                daily_btn.enabled = false;
                break;
            default:
                break;
        }
    }
    public void OnClickClaimButton()
    {
        Debug.Log("On Click Claim button");
    }
}