
using System;
using UnityEngine;
using UnityEngine.UI;

public enum DialogIndex
{
    SettingDialog = 0,
    PauseDialog = 1,
    LoseDialog = 2,
    WinDialog = 3,
    ReviveDialog =4,
    BuyConfirmDialog = 5,
}

public class DialogParam { }

public class SettingDialogParam : DialogParam
{
    public bool musicSetting;
    public bool sfxSetting;
}

public class PauseDialogParam : DialogParam
{
    public bool musicSetting;
    public bool sfxSetting;
}

public class ReviveDialogParam : DialogParam 
{
    public int levelNum;
}

public class LoseDialogParam : DialogParam
{
    public int levelNum;
    public int score;
    public int bestScore;
}

public class WinDialogParam : DialogParam
{
   public int crLevel;
   public int nextLv;
   public int score;
   public int star;
}

public class BuyConfirmDialogParam : DialogParam
{
    //public Action onConfirmAction;
    //public Action onCancleAction;
    public string amount_lb;
    public string bonus_lb;
}
public class LevelConfirm : DialogParam
{
    public string levelnum;
}

public class DialogConfig
{
    public static DialogIndex[] dialogArray = {
        DialogIndex.SettingDialog,
        DialogIndex.PauseDialog,
        DialogIndex.LoseDialog,
        DialogIndex.WinDialog,
        DialogIndex.ReviveDialog,
        DialogIndex.BuyConfirmDialog,
    };
}
