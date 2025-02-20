using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private IngameController ingameController;
    [SerializeField] private DayTimeController dayTimeController;
    public UIRootControlScale UIRoot;
    [SerializeField] private int languageID;
    [SerializeField] private int trackLevelStart;

    public int LanguageID { get => languageID; set => languageID = value; }
    public int TrackLevelStart { get=> trackLevelStart; set => trackLevelStart = value; }
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UIRoot = GetComponentInParent<UIRootControlScale>();    
        SetUpCamera();
    }   

    // Update is called once per frame
    public void SetupGameManager()
    {
        ingameController = FindObjectOfType<IngameController>();
        dayTimeController = FindObjectOfType<DayTimeController>();
        dayTimeController.enabled = true;
        //dayTimeController.CheckNewDay();
    }
    public void LoadIngameSence()
    {
        CameraMain.instance.main.gameObject.SetActive(true);
        LoadSceneManager.instance.LoadSceneByName("InGame", () =>
        {
            ingameController.enabled = true;
            ViewManager.Instance.SwitchView(ViewIndex.GamePlayView, null, () =>
            {
                Debug.Log("LoadIngameSence");
                ingameController.isPause = false;
                ingameController.SetUpIngame();
                SoundManager.Instance.PlayMusic(SoundManager.Music.GamplayMusic);

            });
        });
        DialogManager.Instance.HideAllDialog();
    }
    public void SetUpIngame()
    {
        ingameController.enabled = true;
    }
    public void SetUpCamera()
    {
        GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (camObject == null)
        {
            //Debug.LogError("camObject null");
            CameraMain.instance.GetCamera();
            CameraMain.instance.GetCameraAspect();

        }
        else
        {
            CameraMain.instance.main = camObject.GetComponent<Camera>();
            CameraMain.instance.GetCameraAspect();
        }
    }
}
