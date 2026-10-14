/// <summary>
/// 游戏事件名称集合。
/// 统一管理项目中使用的事件名称，
/// 避免在不同脚本中重复手写字符串。
/// </summary>
public static class GameEvents
{
    /// <summary>
    /// 切换背包界面事件。
    /// </summary>
    public const string ToggleInventory = "ToggleInventory";

    /// <summary>
    /// 切换设置界面事件。
    /// </summary>
    public const string ToggleSettings = "ToggleSettings";
    
    /// <summary>
    /// 金币数量发生变化事件。
    /// </summary>
    public const string GoldChanged = "GoldChanged";
    
    /// <summary>
    /// 请求加载指定场景事件。
    /// </summary>
    public const string LoadScene = "LoadScene";
}
