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
        _eventManager.Unsubscribe(GameEvents.ToggleInventory, ToggleInventory);

        // 取消设置界面事件订阅
        _eventManager.Unsubscribe(GameEvents.ToggleSettings, ToggleSettings);
        
        // 取消关闭设置事件订阅
        _eventManager.Unsubscribe(GameEvents.CloseSettings, CloseSettings);
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

        //订阅事件
        _eventManager.Subscribe(GameEvents.ToggleInventory, ToggleInventory);
        _eventManager.Subscribe(GameEvents.ToggleSettings, ToggleSettings);
        _eventManager.Subscribe(GameEvents.CloseSettings, CloseSettings);
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
                
            if (_inventoryUI != null)
            {
                _inventoryUI.transform.SetParent(transform,false);
            }
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
    /// 首次打开时创建根对象、挂载脚本并初始化；
    /// 之后复用已有对象，只恢复显示。
    /// </summary>
    public void OpenSettings()
    {
        // 仅在首次打开时创建和初始化
        if (_settingsUI == null)
        {
            _settingsUI = new GameObject("SettingsUI", typeof(RectTransform));
            _settingsUI.transform.SetParent(transform, false);
            
            SettingsUI settingsUI = _settingsUI.AddComponent<SettingsUI>();
            settingsUI.Initialize(_eventManager);
        }
        
        // 首次打开或再次打开，都显示已有界面
        _settingsUI.SetActive(true); 
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
    
    // ==================== 测试方法 ====================
    
    private void UpdateGold(int gold)
    {
        Debug.Log("UpdataGold: " + gold);
    }
}