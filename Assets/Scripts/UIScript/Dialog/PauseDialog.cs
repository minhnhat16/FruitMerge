using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseDialog : BaseDialog
{
    [SerializeField] private bool isMusicOn;
    [SerializeField] private bool isSFXOn;
    [SerializeField] private bool isVibOn;

    [SerializeField] private Image musicOn;
    [SerializeField] private Image musicOff;
    [SerializeField] private Image sfxOn;
    [SerializeField] private Image sfxOff;
    [SerializeField] private Image vibOn;
    [SerializeField] private Image vibOff;


    [HideInInspector]
    public UnityEvent<bool> musicEvent = new UnityEvent<bool>();
    [HideInInspector]
    public UnityEvent<bool> sfxEvent = new UnityEvent<bool>();
    [HideInInspector]
    public UnityEvent<bool> vibEvent = new UnityEvent<bool>();
    private void OnEnable()
    {
        musicEvent.AddListener(MusicChange);
        sfxEvent.AddListener(SFXChange);
    }
    private void OnDisable()
    {
        musicEvent.RemoveListener(MusicChange);
        sfxEvent.RemoveListener(SFXChange);

    }
    public override void OnStartShowDialog()
    {
        ZenSDK.instance.ShowFullScreen();
    }
    public void PlayButton()
    {
        IngameController.instance.isPause = true;
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog, () =>
        {
            //EndlessLevel.Instance.RandomCircle();
        IngameController.instance.isPause = false;

        });

    }
    public void HomeButton()
    {
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog);
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
            CameraMain.instance.main.gameObject.SetActive(false);

            Debug.Log("LOAD SCENE BUFFER FROM QUIT");
            ViewManager.Instance.SwitchView(ViewIndex.MainScreenView, null, () =>
            {
                EndlessLevel.Instance.Clear();
            });
        });
    }
    public void MusicChange(bool isOn)
    {
        Debug.Log("MUSIC CHANGED" + isOn);
        SoundManager.Instance.musicSetting = isOn;
        SoundManager.Instance.SettingMusicVolume(isOn);
        if (isOn)
        {

            musicOn.gameObject.SetActive(true);
            musicOff.gameObject.SetActive(false);
        }
        else
        {
            musicOn.gameObject.SetActive(false);
            musicOff.gameObject.SetActive(true);
        }
    }
    public void SFXChange(bool isOn)
    {
        Debug.Log("SFX CHANGED" + isOn);
        SoundManager.Instance.sfxSetting = isOn;
        SoundManager.Instance.SettingSFXVolume(isOn);
        if (isOn)
        {
            sfxOn.gameObject.SetActive(true);
            sfxOff.gameObject.SetActive(false);
        }
        else
        {
            sfxOn.gameObject.SetActive(false);
            sfxOff.gameObject.SetActive(true);
        }
    }
    public void OnMusicChanged()
    {
        isMusicOn = !isMusicOn;
        Debug.Log("OnMusicChanged" + isMusicOn);
        musicEvent?.Invoke(isMusicOn);
    }
    public void OnSFXChanged()
    {
        isSFXOn = !isSFXOn;
        sfxEvent?.Invoke(isSFXOn);
    }    
    public void RestartButton()
    {
        if (ViewManager.Instance.currentView.viewIndex != ViewIndex.GamePlayView) return;
        DialogManager.Instance.HideDialog(DialogIndex.PauseDialog, () =>
        {
            EndlessLevel.Instance.Clear();
            LoadSceneManager.instance.LoadSceneByName("Ingame", () =>
            {
                ViewManager.Instance.SwitchView(ViewIndex.GamePlayView, null, () =>
                {
                    EndlessLevel.Instance.LoadLevel(() =>
                    {
                        IngameController.instance.isPause = false;
                        IngameController.instance.ResetScore();
                    });
                });
            });
        });
        
    }
    public void CloseBtn()
    {
        IngameController.instance.isPause = true;
        DialogManager.Instance.HideDialog(dialogIndex, () =>
        {
            //EndlessLevel.Instance.RandomCircle();
            IngameController.instance.isPause = false;
        });


    }
}
