using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置界面组件。
/// 使用预制体中绑定的按钮，向事件系统发布关闭设置界面的请求。
/// </summary>
public class SettingsUI : MonoBehaviour
{
    // ==================== 字段 ====================

    // 预制体配置：在 SettingsUI 预制体中绑定关闭按钮。
    [SerializeField] private Button _closeButton;

    // 运行时引用：由 UIManager 在创建界面实例后传入。
    private EventManager _eventManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 组件销毁时移除本脚本添加的按钮监听。
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
    /// 保存事件管理器引用，检查预制体按钮配置，并绑定关闭按钮的点击回调。
    /// </summary>
    /// <param name="eventManager">用于发布界面关闭请求的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;

        if (_closeButton == null)
        {
            Debug.LogError("请在 SettingsUI 预制体中绑定关闭按钮。", this);
            return;
        }

        // 先移除本回调，再重新添加，避免重复初始化造成重复监听。
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // ==================== 按钮事件响应 ====================

    /// <summary>
    /// 响应关闭按钮点击，发布 CloseSettings 请求，由 UIManager 隐藏界面。
    /// </summary>
    private void OnCloseButtonClicked()
    {
        _eventManager.Publish(GameEvents.CloseSettings);
    }
}
