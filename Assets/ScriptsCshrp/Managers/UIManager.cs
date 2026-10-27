using UnityEngine;
using System.Collections;

/// <summary>
/// 管理界面显示与交互提示计时，更新玩家操作权限。
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

    // _gameStateManager：用于在设置开关时修改玩家操作权限。
    private GameStateManager _gameStateManager;

    // _interactionUI：已创建的交互提示界面组件，用于更新文字并复用界面。
    private InteractionUI _interactionUI;

    // _interactionDisplayDuration：每次交互后提示保留的现实秒数。
    [SerializeField] private float _interactionDisplayDuration = 3f;

    // _interactionHideCoroutine：当前隐藏计时的协程引用，用于停止旧计时。
    private Coroutine _interactionHideCoroutine;

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
    /// <param name="gameStateManager">用于在设置开关时修改玩家操作权限的状态管理器。</param>
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
    /// 创建或复用设置界面，显示后禁止玩家操作。
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

        // 第四步：显示设置界面，并禁止玩家操作。
        _settingsUI.SetActive(true);
        _gameStateManager.SetPlayerControl(false);
    }

    /// <summary>
    /// 切换设置界面的显示状态，并更新玩家操作权限。
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

    /// <summary>
    /// 创建或复用交互提示，更新文字并重新计时隐藏。
    /// </summary>
    /// <param name="text">需要显示的交互提示文字。</param>
    public void ShowInteractionText(string text)
    {
        // 第一步：首次显示时创建界面。
        if (_interactionUI == null)
        {
            // interactionObject：新创建的交互提示界面实例。
            GameObject interactionObject =
                _resourceManager.InstantiatePrefab("UI/InteractionUI");

            if (interactionObject == null)
            {
                return;
            }

            // 第二步：获取并检查文字控制组件。
            // interactionUI：界面实例上的文字控制组件。
            InteractionUI interactionUI =
                interactionObject.GetComponent<InteractionUI>();
            if (interactionUI == null)
            {
                Debug.LogError("InteractionUI 预制体没有挂载 InteractionUI 脚本。");
                Destroy(interactionObject);
                return;
            }

            // 第三步：设置父对象，并保存组件引用。
            interactionObject.transform.SetParent(transform, false);
            _interactionUI = interactionUI;
        }

        // 第四步：更新文字，并显示界面。
        _interactionUI.SetText(text);
        _interactionUI.gameObject.SetActive(true);

        // 第五步：停止上一次等待，避免旧计时提前隐藏新提示。
        if (_interactionHideCoroutine != null)
        {
            StopCoroutine(_interactionHideCoroutine);
        }

        // 第六步：启动新的等待，并保存协程引用。
        _interactionHideCoroutine =
            StartCoroutine(HideInteractionTextAfterDelay());
    }

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 按现实时间等待后隐藏提示，并清除协程引用。
    /// </summary>
    /// <returns>供 StartCoroutine 执行的协程迭代器。</returns>
    private IEnumerator HideInteractionTextAfterDelay()
    {
        // 第一步：按现实经过的时间等待。
        yield return new WaitForSecondsRealtime(_interactionDisplayDuration);

        // 第二步：界面仍存在时，将它隐藏。
        if (_interactionUI != null)
        {
            _interactionUI.gameObject.SetActive(false);
        }

        // 第三步：清除已经完成的协程引用。
        _interactionHideCoroutine = null;
    }

    // ==================== 事件响应 ====================

    /// <summary>
    /// 隐藏设置界面，并恢复玩家操作。
    /// </summary>
    private void CloseSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(false);
            _gameStateManager.SetPlayerControl(true);
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
