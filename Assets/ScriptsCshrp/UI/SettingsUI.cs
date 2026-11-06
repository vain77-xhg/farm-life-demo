using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 监听设置界面的关闭按钮并发布关闭请求。
/// </summary>
public class SettingsUI : UIBase
{
    // ==================== 字段 ====================

    // _closeButton：预制体中的关闭按钮，在 Inspector 中绑定。
    [SerializeField] private Button _closeButton;

    // _eventManager：由 UIManager 传入，用于发布关闭设置请求。
    private EventManager _eventManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 移除本组件添加的按钮点击监听。
    /// </summary>
    private void OnDestroy()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存事件管理器并绑定关闭按钮。
    /// </summary>
    /// <param name="eventManager">用于发布关闭设置请求的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        // 第一步：保存事件管理器。
        _eventManager = eventManager;

        // 第二步：确认关闭按钮已经绑定。
        if (_closeButton == null)
        {
            Debug.LogError("请在 SettingsUI 预制体中绑定关闭按钮。", this);
            return;
        }

        // 第三步：移除旧监听，再添加本次监听。
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    /// <summary>
    /// 点击按钮时发布关闭设置界面的请求。
    /// </summary>
    private void OnCloseButtonClicked()
    {
        _eventManager.Publish(GameEvents.CloseSettings);
    }

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
