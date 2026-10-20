using UnityEngine;

/// <summary>
/// 界面管理器。
/// 按需加载并复用背包、设置预制体，通过事件响应界面的打开与关闭请求。
/// </summary>
public class UIManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // 由 Bootstrap 传入的管理器引用。
    private ResourceManager _resourceManager;
    private EventManager _eventManager;

    // 首次打开后缓存界面实例，关闭时只隐藏，供下次复用。
    private GameObject _inventoryUI;
    private GameObject _settingsUI;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 组件创建时输出日志；外部管理器引用随后由 Initialize 传入。
    /// </summary>
    private void Awake()
    {
        Debug.Log("UIManager: 初始化完成");
    }

    /// <summary>
    /// 组件销毁时取消界面事件订阅，解除事件管理器保存的回调引用。
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
    /// 保存资源管理器与事件管理器引用，并订阅当前需要响应的界面事件。
    /// </summary>
    /// <param name="resourceManager">用于加载和实例化界面预制体的资源管理器。</param>
    /// <param name="eventManager">用于订阅界面请求的事件管理器。</param>
    public void Initialize(ResourceManager resourceManager, EventManager eventManager)
    {
        _resourceManager = resourceManager;
        _eventManager = eventManager;
        Debug.Log("UIManager 已经接收到 ResourceManager");

        _eventManager.Subscribe(GameEvents.ToggleInventory, ToggleInventory);
        _eventManager.Subscribe(GameEvents.ToggleSettings, ToggleSettings);
        _eventManager.Subscribe(GameEvents.CloseSettings, CloseSettings);
    }

    // ==================== 背包界面 ====================

    /// <summary>
    /// 打开背包界面。
    /// 首次打开时实例化预制体并归入当前管理器层级，之后复用缓存的实例。
    /// </summary>
    public void OpenInventory()
    {
        if (_inventoryUI == null)
        {
            _inventoryUI = _resourceManager.InstantiatePrefab("UI/InventoryUI");

            if (_inventoryUI != null)
            {
                _inventoryUI.transform.SetParent(transform, false);
            }
        }

        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏已经创建的背包界面，保留实例供再次打开使用。
    /// </summary>
    public void CloseInventory()
    {
        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(false);
        }
    }

    /// <summary>
    /// 切换背包显示状态，供 ToggleInventory 事件调用。
    /// 界面尚未创建或已经隐藏时打开，否则关闭。
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

    // ==================== 设置界面 ====================

    /// <summary>
    /// 打开设置界面。
    /// 首次打开时加载预制体、检查组件并调用初始化方法，之后复用已有实例。
    /// </summary>
    public void OpenSettings()
    {
        if (_settingsUI == null)
        {
            GameObject settingsObject = _resourceManager.InstantiatePrefab("UI/SettingsUI");
            if (settingsObject == null)
            {
                return;
            }

            SettingsUI settingsUI = settingsObject.GetComponent<SettingsUI>();
            if (settingsUI == null)
            {
                Debug.LogError("SettingsUI 预制体没有挂载 SettingsUI 脚本。");
                Destroy(settingsObject);
                return;
            }

            settingsObject.transform.SetParent(transform, false);
            settingsUI.Initialize(_eventManager);
            _settingsUI = settingsObject;
        }

        _settingsUI.SetActive(true);
    }

    /// <summary>
    /// 隐藏已经创建的设置界面，保留实例供再次打开使用。
    /// 此方法同时作为 CloseSettings 事件的回调。
    /// </summary>
    private void CloseSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(false);
        }
    }

    /// <summary>
    /// 切换设置界面的显示状态，供 ToggleSettings 事件调用。
    /// 界面尚未创建或已经隐藏时打开，否则关闭。
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

    // ==================== 预留测试 ====================

    /// <summary>
    /// 输出金币数值的测试日志，目前尚未接入金币变化事件的订阅流程。
    /// </summary>
    /// <param name="gold">需要输出的金币数量。</param>
    private void UpdateGold(int gold)
    {
        Debug.Log("UpdataGold: " + gold);
    }
}
