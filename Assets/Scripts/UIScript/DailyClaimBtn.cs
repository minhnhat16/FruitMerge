using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyClaimBtn : MonoBehaviour
{
    List<Button> dailyButtons = new List<Button>();
    private bool isClaimed;

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
        if(isClaimed) return;
        else
        {
            dailyButtons[0].gameObject.SetActive(true);
        }
    }
    public void ClaimBtn()
    {
        Debug.Log("Claim reward");
    }
    public void AdsBtn()
    {
        Debug.Log("Ads Btn");
    }
}
