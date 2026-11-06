using UnityEngine;
using System.Collections;

/// <summary>
/// 协调界面切换、交互提示计时和玩家操作权限。
/// </summary>
public class UIManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _resourceManager：资源管理器组件，用于创建界面实例。
    private ResourceManager _resourceManager;

    // _eventManager：事件管理器组件，用于订阅界面请求。
    private EventManager _eventManager;

    // _inventoryUI：背包界面的控制组件，关闭后保留复用。
    private InventoryUI _inventoryUI;

    // _settingsUI：设置界面的控制组件，关闭后保留复用。
    private SettingsUI _settingsUI;

    // _gameStateManager：用于保存和更新玩家移动、场景交互权限。
    private GameStateManager _gameStateManager;

    // _interactionUI：已创建的交互提示界面组件，用于更新文字并复用界面。
    private InteractionUI _interactionUI;

    // _interactionDisplayDuration：每次交互后提示保留的现实秒数。
    [SerializeField] private float _interactionDisplayDuration = 3f;

    // _interactionHideCoroutine：当前隐藏计时的协程引用，用于停止旧计时。
    private Coroutine _interactionHideCoroutine;

    // _currentUI：通过统一入口显示的界面组件，没有当前记录时为 null。
    private UIBase _currentUI;

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
        _eventManager.UnsubscribeString(GameEvents.ShowInteractionText, ShowInteractionText);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存依赖并订阅界面事件。
    /// </summary>
    /// <param name="resourceManager">用于加载和创建预制体的资源管理器。</param>
    /// <param name="eventManager">用于发布或订阅事件的事件管理器。</param>
    /// <param name="gameStateManager">用于更新玩家移动和场景交互权限的状态管理器。</param>
    public void Initialize(
        ResourceManager resourceManager,
        EventManager eventManager,
        GameStateManager gameStateManager)
    {
        // 第一步：保存资源、事件和状态管理器。
        _resourceManager = resourceManager;
        _eventManager = eventManager;
        _gameStateManager = gameStateManager;
        Debug.Log("UIManager 已经接收到 ResourceManager");

        // 第二步：订阅背包、设置和交互提示的界面事件。
        _eventManager.Subscribe(GameEvents.ToggleInventory, ToggleInventory);
        _eventManager.Subscribe(GameEvents.ToggleSettings, ToggleSettings);
        _eventManager.Subscribe(GameEvents.CloseSettings, CloseSettings);
        _eventManager.SubscribeString(GameEvents.ShowInteractionText, ShowInteractionText);
    }

    // ==================== 对外接口 ====================

    // ---------- 背包界面 ----------

    /// <summary>
    /// 创建或复用并显示背包界面。
    /// </summary>
    public void OpenInventory()
    {
        // 第一步：没有缓存时创建背包组件。
        if (_inventoryUI == null)
        {
            _inventoryUI = CreateUI<InventoryUI>("UI/InventoryUI");
        }

        // 第二步：通过统一入口显示背包。
        ShowUI(_inventoryUI);
    }

    /// <summary>
    /// 关闭当前显示的背包界面。
    /// </summary>
    public void CloseInventory()
    {
        CloseUI(_inventoryUI);
    }

    /// <summary>
    /// 切换背包界面的显示状态。
    /// </summary>
    public void ToggleInventory()
    {
        // 当前正在显示背包时关闭，否则打开。
        if (_inventoryUI != null && _currentUI == _inventoryUI)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    // ---------- 设置界面 ----------

    /// <summary>
    /// 创建或复用并显示设置界面。
    /// </summary>
    public void OpenSettings()
    {
        // 第一步：没有缓存时创建并检查设置组件。
        if (_settingsUI == null)
        {
            _settingsUI = CreateUI<SettingsUI>("UI/SettingsUI");

            if (_settingsUI == null)
            {
                return;
            }

            // 第二步：首次创建成功后，初始化设置界面。
            _settingsUI.Initialize(_eventManager);
        }

        // 第三步：通过统一入口显示设置。
        ShowUI(_settingsUI);
    }

    /// <summary>
    /// 关闭当前设置界面并恢复玩家操作。
    /// </summary>
    private void CloseSettings()
    {
        CloseUI(_settingsUI);
    }

    /// <summary>
    /// 切换设置界面的显示状态。
    /// </summary>
    public void ToggleSettings()
    {
        // 当前正在显示设置时关闭，否则打开。
        if (_settingsUI != null && _currentUI == _settingsUI)
        {
            CloseSettings();
        }
        else
        {
            OpenSettings();
        }
    }

    // ---------- 交互提示 ----------

    /// <summary>
    /// 显示交互提示并重新计时隐藏。
    /// </summary>
    /// <param name="text">需要显示的交互提示文字。</param>
    public void ShowInteractionText(string text)
    {
        // 第一步：没有缓存时创建提示，失败则结束。
        if (_interactionUI == null)
        {
            _interactionUI = CreateUI<InteractionUI>("UI/InteractionUI");

            if (_interactionUI == null)
            {
                return;
            }
        }

        // 第二步：更新文字，通过统一入口显示。
        _interactionUI.SetText(text);
        ShowUI(_interactionUI);

        // 第三步：停止上一次隐藏计时。
        if (_interactionHideCoroutine != null)
        {
            StopCoroutine(_interactionHideCoroutine);
        }

        // 第四步：启动新计时并保存协程引用。
        _interactionHideCoroutine = StartCoroutine(HideInteractionTextAfterDelay());
    }

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 按现实时间等待后关闭交互提示。
    /// </summary>
    /// <returns>供 StartCoroutine 执行的协程迭代器。</returns>
    private IEnumerator HideInteractionTextAfterDelay()
    {
        // 第一步：按现实时间等待。
        yield return new WaitForSecondsRealtime(_interactionDisplayDuration);

        // 第二步：等待结束，清空协程引用。
        _interactionHideCoroutine = null;

        // 第三步：通过统一入口关闭提示。
        CloseUI(_interactionUI);
    }

    // ==================== 事件响应 ====================

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

    /// <summary>
    /// 创建并准备指定类型的界面组件。
    /// </summary>
    /// <typeparam name="T">需要获取的界面组件类型。</typeparam>
    /// <param name="path">预制体的 Resources 路径。</param>
    /// <returns>成功返回界面组件，失败返回 null。</returns>
    private T CreateUI<T>(string path) where T : UIBase
    {
        // 第一步：创建并检查预制体实例。
        // uiObject：本次创建的界面根对象。
        GameObject uiObject = _resourceManager.InstantiatePrefab(path);
        if (uiObject == null)
        {
            return null;
        }

        // 第二步：获取并检查界面组件。
        // uiComponent：界面根对象上指定类型的控制组件。
        T uiComponent = uiObject.GetComponent<T>();
        if (uiComponent == null)
        {
            Debug.LogError("UI 预制体缺少对应的界面脚本：" + path);
            Destroy(uiObject);
            return null;
        }

        // 第三步：设置父对象，并将界面隐藏待用。
        uiObject.transform.SetParent(transform, false);
        uiComponent.Hide();

        // 第四步：返回准备好的界面组件。
        return uiComponent;
    }

    /// <summary>
    /// 停止提示计时，隐藏当前界面并清空记录。
    /// </summary>
    private void CloseCurrentUI()
    {
        // 第一步：没有当前界面时，直接结束方法。
        if (_currentUI == null)
        {
            return;
        }

        // 第二步：存在提示计时时，停止协程并清空引用。
        if (_interactionHideCoroutine != null)
        {
            StopCoroutine(_interactionHideCoroutine);
            _interactionHideCoroutine = null;
        }

        // 第三步：隐藏当前界面。
        _currentUI.Hide();

        // 第四步：清空当前界面记录。
        _currentUI = null;
    }

    /// <summary>
    /// 切换显示界面并应用目标界面的操作规则。
    /// </summary>
    /// <param name="targetUI">准备显示的界面控制组件。</param>
    private void ShowUI(UIBase targetUI)
    {
        // 第一步：目标为空时，保留当前界面并结束方法。
        if (targetUI == null)
        {
            return;
        }

        // 第二步：关闭原来的界面。
        CloseCurrentUI();

        // 第三步：显示目标界面。
        targetUI.Show();

        // 第四步：将目标保存到当前界面记录。
        _currentUI = targetUI;

        // 第五步：根据目标界面的规则更新玩家操作权限。
        _gameStateManager.SetPlayerControl(targetUI.CanPlayerControl());
    }

    /// <summary>
    /// 关闭指定的当前界面并恢复玩家操作。
    /// </summary>
    /// <param name="targetUI">请求关闭的界面控制组件。</param>
    private void CloseUI(UIBase targetUI)
    {
        // 第一步：目标为空或已经不是当前界面时，结束方法。
        if (targetUI == null || targetUI != _currentUI)
        {
            return;
        }

        // 第二步：隐藏当前界面并清空记录。
        CloseCurrentUI();

        // 第三步：恢复玩家操作。
        _gameStateManager.SetPlayerControl(true);
    }

    // ==================== 调试显示 ====================
}
