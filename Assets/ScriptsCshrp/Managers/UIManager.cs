using UnityEngine;

/// <summary>
/// 创建并复用界面，控制界面的显示和隐藏。
/// </summary>
public class UIManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _resourceManager：资源管理器组件，用于创建界面实例。
    private ResourceManager _resourceManager;

    // _eventManager：事件管理器组件，用于订阅界面请求。
    private EventManager _eventManager;

    // _inventoryUI：已创建的背包界面实例，关闭后保留复用。
    private GameObject _inventoryUI;

    // _settingsUI：已创建的设置界面实例，关闭后保留复用。
    private GameObject _settingsUI;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 输出界面管理器启动日志。
    /// </summary>
    private void Awake()
    {
        Debug.Log("UIManager: 初始化完成");
    }

    /// <summary>
    /// 取消本组件订阅的界面事件。
    /// </summary>
    void OnDestroy()
    {
        if (_eventManager == null)
        {
            return;
        }

        _eventManager.Unsubscribe(GameEvents.ToggleInventory, ToggleInventory);
        _eventManager.Unsubscribe(GameEvents.ToggleSettings, ToggleSettings);
        _eventManager.Unsubscribe(GameEvents.CloseSettings, CloseSettings);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存依赖并订阅界面事件。
    /// </summary>
    /// <param name="resourceManager">用于加载和创建预制体的资源管理器。</param>
    /// <param name="eventManager">用于发布或订阅事件的事件管理器。</param>
    public void Initialize(ResourceManager resourceManager, EventManager eventManager)
    {
        // 第一步：保存资源和事件管理器。
        _resourceManager = resourceManager;
        _eventManager = eventManager;
        Debug.Log("UIManager 已经接收到 ResourceManager");

        // 第二步：订阅背包和设置界面的操作事件。
        _eventManager.Subscribe(GameEvents.ToggleInventory, ToggleInventory);
        _eventManager.Subscribe(GameEvents.ToggleSettings, ToggleSettings);
        _eventManager.Subscribe(GameEvents.CloseSettings, CloseSettings);
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 首次创建背包，之后复用并显示。
    /// </summary>
    public void OpenInventory()
    {
        // 第一步：首次打开时创建背包实例。
        if (_inventoryUI == null)
        {
            _inventoryUI = _resourceManager.InstantiatePrefab("UI/InventoryUI");

            if (_inventoryUI != null)
            {
                // 第二步：创建成功后设置父对象。
                _inventoryUI.transform.SetParent(transform, false);
            }
        }

        if (_inventoryUI != null)
        {
            // 第三步：显示背包。
            _inventoryUI.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏已有的背包界面。
    /// </summary>
    public void CloseInventory()
    {
        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(false);
        }
    }

    /// <summary>
    /// 切换背包界面的显示状态。
    /// </summary>
    public void ToggleInventory()
    {
        // 未创建或已隐藏时打开，否则关闭。
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
    /// 首次创建并初始化设置界面，之后复用。
    /// </summary>
    public void OpenSettings()
    {
        // 第一步：首次打开时创建设置界面。
        if (_settingsUI == null)
        {
            // settingsObject：新创建的设置界面实例。
            GameObject settingsObject = _resourceManager.InstantiatePrefab("UI/SettingsUI");
            if (settingsObject == null)
            {
                return;
            }

            // 第二步：获取并检查设置界面组件。
            // settingsUI：设置界面实例上的控制组件。
            SettingsUI settingsUI = settingsObject.GetComponent<SettingsUI>();
            if (settingsUI == null)
            {
                Debug.LogError("SettingsUI 预制体没有挂载 SettingsUI 脚本。");
                Destroy(settingsObject);
                return;
            }

            // 第三步：设置父对象、初始化并保存实例。
            settingsObject.transform.SetParent(transform, false);
            settingsUI.Initialize(_eventManager);
            _settingsUI = settingsObject;
        }

        // 第四步：显示设置界面。
        _settingsUI.SetActive(true);
    }

    /// <summary>
    /// 切换设置界面的显示状态。
    /// </summary>
    public void ToggleSettings()
    {
        // 未创建或已隐藏时打开，否则关闭。
        if (_settingsUI == null || !_settingsUI.activeSelf)
        {
            OpenSettings();
        }
        else
        {
            CloseSettings();
        }
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    /// <summary>
    /// 响应关闭请求，隐藏设置界面。
    /// </summary>
    private void CloseSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(false);
        }
    }

    // 预留：当前尚未订阅金币变化事件。
    /// <summary>
    /// 输出金币数量的测试日志。
    /// </summary>
    /// <param name="gold">需要输出的金币数量。</param>
    private void UpdateGold(int gold)
    {
        Debug.Log("UpdataGold: " + gold);
    }

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
