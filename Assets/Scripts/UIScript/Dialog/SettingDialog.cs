using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingDialog : BaseDialog
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
    [SerializeField] private TMP_Dropdown language_dr;

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
        language_dr.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void OnDisable()
    {
        musicEvent.RemoveListener(MusicChange);
        sfxEvent.RemoveListener(SFXChange);

    }
    public override void OnStartShowDialog()
    {
        base.OnStartShowDialog();
       
        Player.instance.gameObject.SetActive(false);
        EndlessLevel.Instance.main.gameObject.SetActive(false);
    }
    public override void OnEndHideDialog()
    {
        base.OnEndHideDialog();
        Player.instance.canDrop = true;
        //Player.instance.gameObject.SetActive(true);
        EndlessLevel.Instance.main.gameObject.SetActive(true);

    }
    public void PlayButton()
    {
        IngameController.instance.isPause = false;
        DialogManager.Instance.HideDialog(dialogIndex, () =>
        {
            //EndlessLevel.Instance.RandomCircle();
        });

    }
    public void HomeButton()
    {
        DialogManager.Instance.HideDialog(dialogIndex);
        LoadSceneManager.instance.LoadSceneByName("Buffer", () =>
        {
            CameraMain.instance.main.gameObject.SetActive(false);

            Debug.Log("LOAD SCENE BUFFER FROM QUIT");
            DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog, null, () =>
            {
                EndlessLevel.Instance.Clear();
                CameraMain.instance.main.gameObject.SetActive(true);
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
        DialogManager.Instance.HideDialog(dialogIndex);
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
    }
    public void CloseBtn()
    {
        Debug.Log("Close button on " + this.dialogIndex);
        DialogManager.Instance.HideDialog(this.dialogIndex);
        IngameController.instance.isPause = false;
    }
    private void OnDropdownValueChanged(int index)
    {
        // Get the selected option text
        string selectedOption =language_dr.options[index].text;

        // Display the selected option
        Debug.Log("Selected Option: " + selectedOption + " with index " + language_dr.options[index]);
    }
    
}
