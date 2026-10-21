/// <summary>
/// 保存发布和订阅时使用的事件名称。
/// </summary>
public static class GameEvents
{
    // ==================== 字段 ====================

    /// <summary>
    /// ToggleInventory：切换背包界面的事件名称，无参数。
    /// </summary>
    public const string ToggleInventory = "ToggleInventory";

    /// <summary>
    /// ToggleSettings：切换设置界面的事件名称，无参数。
    /// </summary>
    public const string ToggleSettings = "ToggleSettings";

    /// <summary>
    /// CloseSettings：关闭设置界面的事件名称，无参数。
    /// </summary>
    public const string CloseSettings = "CloseSettings";

    /// <summary>
    /// LoadScene：加载场景的事件名称，使用 string 传递场景名。
    /// </summary>
    public const string LoadScene = "LoadScene";

    /// <summary>
    /// GoldChanged：预留的金币变化事件名称，计划传递 int 数据。
    /// </summary>
    public const string GoldChanged = "GoldChanged";

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
