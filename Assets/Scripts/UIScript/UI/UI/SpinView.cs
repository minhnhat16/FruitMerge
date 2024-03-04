using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinView : BaseView
{
    [SerializeField] SpinCircle spiner;
    [SerializeField] Button button;
    public override void OnStartShowView()
    {
        base.OnStartShowView();
        spiner = GetComponent<SpinCircle>();
        NewDayCheck();
    }
    private void Update()
    {
      
    }
    public void NewDayCheck()
    {
        if (DayTimeController.instance.isNewDay)
        {
            button.gameObject.SetActive(true);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }
}

