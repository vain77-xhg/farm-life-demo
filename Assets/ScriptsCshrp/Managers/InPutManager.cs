using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 输入管理器。
/// 发布界面切换事件，并提供移动、奔跑和交互输入的查询方法。
/// 具体的玩家行为由 PlayerController 执行。
/// </summary>
public class InPutManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // 由 Bootstrap 传入，用于发布界面事件。
    private EventManager _eventManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 每帧检测界面快捷键与鼠标点击。
    /// Tab 和 Escape 发布界面切换事件，鼠标点击暂时输出所在区域的测试日志。
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("检测到Tab按键按下");
            _eventManager.Publish(GameEvents.ToggleInventory);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("检测到ESC键按下");
            _eventManager.Publish(GameEvents.ToggleSettings);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("检测到鼠标左键按下");
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("鼠标点击发生在UI上");
                return;
            }
            else
            {
                Debug.Log("鼠标点击发生在UI外");
                // 后续在此接入游戏世界的鼠标点击行为。
            }
        }
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存事件管理器引用，供界面快捷键发布事件。
    /// </summary>
    /// <param name="eventManager">Bootstrap 已经创建的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    // ==================== 移动输入 ====================

    /// <summary>
    /// 读取 Horizontal 和 Vertical 输入轴，组成未经平滑处理的二维方向。
    /// 具体按键由项目的 Input Manager 配置决定。
    /// </summary>
    /// <returns>包含横向与纵向输入的向量；无输入时为零向量。</returns>
    public Vector2 GetMoveDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector2(horizontal, vertical);
    }

    /// <summary>
    /// 查询左 Shift 奔跑键是否持续按住。
    /// </summary>
    /// <returns>按住时返回 true，否则返回 false。</returns>
    public bool IsRunPressed()
    {
        bool isRunPressed = Input.GetKey(KeyCode.LeftShift);
        return isRunPressed;
    }

    // ==================== 交互输入 ====================

    /// <summary>
    /// 查询当前帧是否刚按下 E 交互键，供玩家在 Update 中检测。
    /// </summary>
    /// <returns>刚按下的这一帧返回 true，其余帧返回 false。</returns>
    public bool IsInteractPressed()
    {
        bool isInteractPressed = Input.GetKeyDown(KeyCode.E);
        return isInteractPressed;
    }
}
