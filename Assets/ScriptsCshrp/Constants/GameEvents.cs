/// <summary>
/// 游戏事件名称集合。
/// 发布与订阅时共用这些常量，避免在不同脚本中重复手写事件名称。
/// </summary>
public static class GameEvents
{
    // ==================== 界面事件 ====================

    /// <summary>
    /// 切换背包界面的显示状态，无参数。
    /// </summary>
    public const string ToggleInventory = "ToggleInventory";

    /// <summary>
    /// 切换设置界面的显示状态，无参数。
    /// </summary>
    public const string ToggleSettings = "ToggleSettings";

    /// <summary>
    /// 请求关闭设置界面，无参数。
    /// </summary>
    public const string CloseSettings = "CloseSettings";

    // ==================== 场景事件 ====================

    /// <summary>
    /// 请求加载场景，使用 string 参数传递目标场景名称。
    /// </summary>
    public const string LoadScene = "LoadScene";

    // ==================== 预留数据事件 ====================

    /// <summary>
    /// 预留的金币变化事件，计划使用 int 参数传递金币数量。
    /// 当前尚未接入发布与订阅流程。
    /// </summary>
    public const string GoldChanged = "GoldChanged";
}
