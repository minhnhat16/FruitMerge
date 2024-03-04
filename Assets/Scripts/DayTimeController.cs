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
        DateTime last = DateTime.Parse(DataAPIController.instance.GetDayTimeData());
        if (DateTime.Today >last.Date)
        {
            Debug.Log("NEW DAY HAS STARTED");   
            isNewDay = true;
            SetNewDay();
            newDateEvent?.Invoke(isNewDay);
        }
        else
        {
            //isNewDay = false;
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
