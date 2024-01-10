

public enum ViewIndex
{
    EmptyView = 0,
    LoadingView = 1,
    MainScreenView = 2,
    GamePlayView = 3,
    LevelView = 4,
    ShopView = 5,
    SpinView = 6,
    DailyView = 7,
    SelectLevelView = 8
}

public class ViewParam { }

public class LoadingViewParam : ViewParam
{ 
}
public class GamePlayViewParam : ViewParam
{
}
public class ViewConfig
{
    public static ViewIndex[] viewArray = {
        ViewIndex.EmptyView,
        ViewIndex.LoadingView,
        ViewIndex.MainScreenView,
        ViewIndex.GamePlayView,
        ViewIndex.LevelView,
        ViewIndex.ShopView,
        ViewIndex.DailyView,
        ViewIndex.SpinView,
        ViewIndex.SelectLevelView 
    };
}