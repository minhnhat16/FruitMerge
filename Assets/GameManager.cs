using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private IngameController ingameController;
    public UIRootControlScale UIRoot;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetupGameManager()
    {
        ingameController = FindObjectOfType<IngameController>();
    }
    public void LoadBufferScene()
    {
        if (ingameController.isActiveAndEnabled)
        {
            ingameController.enabled = false;
        }
    }
    public void LoadIngameSence()
    {
        CameraMain.instance.main.gameObject.SetActive(true);

        LoadSceneManager.instance.LoadSceneByName("InGame", () =>
        {
            ingameController.enabled = true;
            ViewManager.Instance.SwitchView(ViewIndex.GamePlayView, null, () =>
            {
                //Debug.Log("LoadIngameSence");
                ingameController.isPause = false;
                ingameController.SetUpIngame();
                SoundManager.Instance.PlayMusic(SoundManager.Music.GamplayMusic);
            });

        });


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
