using UnityEngine;

/// <summary>
/// 保存并提供玩家移动和场景交互的操作权限。
/// </summary>
public class GameStateManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _canPlayerControl：当前生效的移动和场景交互权限，由 UIManager 更新。
    private bool _canPlayerControl = true;

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 提供当前生效的玩家操作权限。
    /// </summary>
    /// <returns>允许移动和场景交互时为 true，否则为 false。</returns>
    public bool CanPlayerControl()
    {
        return _canPlayerControl;
    }

    /// <summary>
    /// 设置是否允许玩家移动和进行场景交互。
    /// </summary>
    /// <param name="canControl">需要保存的操作权限，true 表示允许。</param>
    public void SetPlayerControl(bool canControl)
    {
        _canPlayerControl = canControl;
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
