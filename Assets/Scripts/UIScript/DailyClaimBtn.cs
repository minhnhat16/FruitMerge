using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyClaimBtn : MonoBehaviour
{
   public List<Button> dailyButtons = new List<Button>();
    private bool isClaimed;
    [HideInInspector] UnityEvent<bool> onClickClaim = new();
    [HideInInspector] UnityEvent<bool> onClickAds = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckButtonType()
    {
        Debug.Log("Check Button Type");
        if(isClaimed) return;
        else
        {
            dailyButtons[0].gameObject.SetActive(true);
        }
    }
    public void SetButtonEvent(UnityEvent<bool> claimEvent, UnityEvent<bool> adsEvent) 
     {
        this.onClickClaim = claimEvent;
        this.onClickAds = adsEvent;
    }

    public void ClaimBtn()
    {
        Debug.Log("Claim reward");
        onClickClaim?.Invoke(true);
    }
    public void AdsBtn()
    {
        Debug.Log("Ads Btn");
        onClickAds?.Invoke(true);
    }
}
