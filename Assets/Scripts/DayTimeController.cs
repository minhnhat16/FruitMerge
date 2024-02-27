using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DayTimeController : MonoBehaviour
{
    public static DayTimeController instance;
    public DateTime lastCheckedDay;
    private void Awake()
    {
        string lastCheckedDateString = PlayerPrefs.GetString("LastCheckedDate", string.Empty);
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
