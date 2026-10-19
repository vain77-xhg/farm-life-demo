using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置界面组件。
/// 使用预制体中配置的按钮，通过事件系统请求关闭界面。
/// </summary>
public class SettingsUI : MonoBehaviour
{
    // ==================== 字段 ====================

    // 在预制体中绑定关闭按钮
    [SerializeField] private Button _closeButton;
    
    // 保存事件管理器引用，用于发布界面事件
    private EventManager _eventManager;

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存事件管理器引用，并绑定关闭按钮的点击行为。
    /// </summary>
    /// <param name="eventManager">用于发布界面事件的事件管理器引用。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;

        if (_closeButton == null)
        {
            Debug.LogError("请在 SettingsUI 预制体中绑定关闭按钮。", this);
            return;
        }

        // 先移除本方法已有的绑定，避免重复初始化时重复添加
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 组件销毁时，解除关闭按钮的点击绑定。
    /// </summary>
    private void OnDestroy()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }

    // ==================== 按钮行为 ====================

    /// <summary>
    /// 处理关闭按钮点击，发布关闭设置界面的请求。
    /// </summary>
    private void OnCloseButtonClicked()
    {
        _eventManager.Publish(GameEvents.CloseSettings);
    }
}
