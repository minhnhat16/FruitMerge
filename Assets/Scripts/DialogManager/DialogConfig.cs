using System;

public enum DialogIndex
{
    LableChooseDialog = 0,
    PauseDialog = 1,
    LoseDialog = 2,
    DailyRewardDialog = 3,
    ReviveDialog = 4,
    BuyConfirmDialog = 5,
    ItemConfirmDialog = 6,
    SettingDialog = 7,
    LeaderBoardDialog = 8,
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
    public int score;
}

public class DailyDialogParam : DialogParam
{
    int totalDay;
    bool isClaimed;
}

public class BuyConfirmDialogParam : DialogParam
{
    public Action onConfirmAction;
    public Action onCancleAction;
    public string amount_lb;
    public string bonus_lb;
    public string cost_lb;
}
public class LevelConfirm : DialogParam
{
    public string levelnum;
}
public class ItemConfirmParam : DialogParam
{
    public int type;
    public string name;
}

public class ShopDialogParam : DialogParam
{

}
public class LeaderBoardDialogParam : DialogParam
{

}
public class DialogConfig
{
    public static DialogIndex[] dialogArray = {
       DialogIndex.LableChooseDialog,
        DialogIndex.PauseDialog,
        DialogIndex.LoseDialog,
        DialogIndex.DailyRewardDialog,
        DialogIndex.ReviveDialog,
        DialogIndex.BuyConfirmDialog,
        DialogIndex.ItemConfirmDialog,
        DialogIndex.SettingDialog,
       DialogIndex.LeaderBoardDialog,
    };
}
