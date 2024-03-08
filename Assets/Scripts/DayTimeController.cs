using System;
using UnityEngine;
using UnityEngine.Events;

public class DayTimeController : MonoBehaviour
{
    public static DayTimeController instance;
    public bool isNewDay;
    public DateTime lastCheckedDay;
    public string lastString;
    public string dataString;
    public UnityEvent<bool> newDateEvent = new UnityEvent<bool>();
    //public UnityEvent<bool>  = new UnityEvent<bool>();

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        newDateEvent.RemoveAllListeners();
    }
    // Start is called before the first frame update
    void Start()
    {
        NewDayEvent();
        lastString = lastCheckedDay.ToString();
        dataString = DataAPIController.instance.GetDayTimeData();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        NewDayEvent();
    }
    public void NewDayEvent()
    {
        string dayTimeData = DataAPIController.instance.GetDayTimeData();
        DateTime last;

        if (dayTimeData != null && DateTime.TryParse(dayTimeData, out last))
        {
            // Use the parsed DateTime value 'last' here
        }
        else
        {
            // Handle the case where dayTimeData is null or parsing failed
            Debug.LogWarning("Failed to parse day time data or data is null.");
        }
    }
    public void SetNewDay()
    {
        //Debug.Log("DAY TIME DATA CHECK  NULL");
        lastCheckedDay = DateTime.Today;
        lastString = lastCheckedDay.ToString();
        DataAPIController.instance.SetDayTimeData(lastCheckedDay.ToString());
    }
    public void NewDay(bool isNew)
    {
        Debug.Log("new day listener"); 
        isNewDay = isNew;
    }
}
