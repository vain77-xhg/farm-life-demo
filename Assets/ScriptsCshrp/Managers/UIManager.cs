using UnityEngine;

/// <summary>
/// 界面管理
/// </summary>
public class UIManager : MonoBehaviour
{
    // ==================== 字段 ====================

    private ResourceManager _resourceManager;
    private EventManager _eventManager;
    private GameObject _inventoryUI;
    private GameObject _settingsUI;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 输出 UIManager 的初始化日志。
    /// </summary>
    private void Awake()
    {
        Debug.Log("UIManager: 初始化完成");
    }

    /// <summary>
    /// UIManager 被销毁时取消事件订阅，
    /// 避免 EventManager 继续保存无效的方法引用。
    /// </summary>
    void OnDestroy()
    {
        // 如果还没有取得 EventManager 引用，就不需要取消订阅
        if (_eventManager == null)
        {
            return;
        }

        // 取消背包界面事件订阅
        _eventManager.UnSubscribe(GameEvents.ToggleInventory, ToggleInventory);

        // 取消设置界面事件订阅
        _eventManager.UnSubscribe(GameEvents.ToggleSettings, ToggleSettings);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 初始化 UI 管理器。
    /// 保存资源管理器和事件管理器引用，
    /// 并订阅需要处理的 UI 事件。
    /// </summary>
    /// <param name="resourceManager">资源管理器引用。</param>
    /// <param name="eventManager">事件管理器引用。</param>
    public void Initialize(ResourceManager resourceManager, EventManager eventManager)
    {
        _resourceManager = resourceManager;
        _eventManager = eventManager;
        Debug.Log("UIManager 已经接收到 ResourceManager");

        _eventManager.Subscribe(GameEvents.ToggleInventory, ToggleInventory);
        _eventManager.Subscribe(GameEvents.ToggleSettings, ToggleSettings);
    }

    // ==================== 背包 UI ====================

    /// <summary>
    /// 切换背包界面的显示状态。
    /// 背包尚未创建或未显示时打开，否则关闭。
    /// </summary>
    public void ToggleInventory()
    {
        if (_inventoryUI == null || !_inventoryUI.activeSelf)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    /// <summary>
    /// 打开背包界面。
    /// 首次打开时通过资源管理器创建预制体，之后复用已有界面。
    /// </summary>
    public void OpenInventory()
    {
        if (_inventoryUI == null)
        {
            _inventoryUI = _resourceManager.InstantiatePrefab("UI/InventoryUI");
        }

        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏已创建的背包界面，保留实例供下次打开使用。
    /// </summary>
    public void CloseInventory()
    {
        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(false);
        }
    }

    // ==================== 设置 UI ====================

    /// <summary>
    /// 切换设置界面的显示状态。
    /// 设置界面尚未创建或未显示时打开，否则关闭。
    /// </summary>
    public void ToggleSettings()
    {
        if (_settingsUI == null || !_settingsUI.activeSelf)
        {
            OpenSettings();
        }
        else
        {
            CloseSettings();
        }
    }

    /// <summary>
    /// 打开设置界面。
    /// 首次打开时通过资源管理器创建预制体，之后复用已有界面。
    /// </summary>
    public void OpenSettings()
    {
        if (_settingsUI == null)
        {
            _settingsUI = _resourceManager.InstantiatePrefab("UI/SettingsUI");
        }
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏已创建的设置界面，保留实例供下次打开使用。
    /// </summary>
    private void CloseSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(false);
        }
    }
}
