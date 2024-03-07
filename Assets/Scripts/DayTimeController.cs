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
        // Attempt to parse the string representation of the date and time
        if (DateTime.TryParse(DataAPIController.instance.GetDayTimeData(), out DateTime last))
        {
            // Parsing successful
            if (DateTime.Today > last.Date)
            {
                Debug.Log("NEW DAY HAS STARTED");
                isNewDay = true;
                SetNewDay();
                newDateEvent?.Invoke(isNewDay);
            }
        }
        else
        {
            // Parsing failed, handle the case where the input string is not in the correct format
            Debug.LogError("Failed to parse DateTime from the input string.");
            // You might want to log an error, notify the user, or take any other appropriate action
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
