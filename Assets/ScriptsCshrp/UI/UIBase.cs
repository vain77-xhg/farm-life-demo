using UnityEngine;

/// <summary>
/// 提供界面显示、隐藏和玩家操作规则查询。
/// </summary>
public class UIBase : MonoBehaviour
{
    // ==================== 字段 ====================

    // _canPlayerControl：显示此界面时，是否允许玩家移动和场景交互。
    [SerializeField] private bool _canPlayerControl = false;

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 查询显示此界面时是否允许玩家操作。
    /// </summary>
    /// <returns>允许移动和场景交互时为 true，否则为 false。</returns>
    public bool CanPlayerControl()
    {
        return _canPlayerControl;
    }

    /// <summary>
    /// 显示本组件所在的界面根对象。
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏本组件所在的界面根对象。
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
