/// <summary>
/// 游戏场景名称集合。
/// 配合 SceneLoader 请求场景加载，名称需要与实际场景保持一致。
/// </summary>
public static class GameScenes
{
    // ==================== 场景名称 ====================

    /// <summary>
    /// 启动场景，包含 Bootstrap，负责创建管理器并进入游戏场景。
    /// </summary>
    public const string SampleScene = "SampleScene";

    /// <summary>
    /// 用于验证场景切换的测试场景。
    /// </summary>
    public const string TestScene = "TestScene";

    /// <summary>
    /// 农场游戏场景，由 FarmSceneController 创建玩家并设置相机跟随目标。
    /// </summary>
    public const string FarmScene = "FarmScene";
}
