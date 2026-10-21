using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 查询玩家输入，发布界面操作事件。
/// </summary>
public class InPutManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _eventManager：事件管理器组件，用于发布界面事件。
    private EventManager _eventManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 检测界面快捷键和鼠标点击。
    /// </summary>
    void Update()
    {
        // 第一步：按 Tab 时发布背包切换事件。
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("检测到Tab按键按下");
            _eventManager.Publish(GameEvents.ToggleInventory);
        }

        // 第二步：按 Escape 时发布设置切换事件。
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("检测到ESC键按下");
            _eventManager.Publish(GameEvents.ToggleSettings);
        }

        // 第三步：点击鼠标时输出所在区域。
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
            }
        }
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存用于发布事件的事件管理器。
    /// </summary>
    /// <param name="eventManager">用于发布或订阅事件的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 读取横向和纵向输入，返回移动方向。
    /// </summary>
    /// <returns>由横向和纵向输入组成的方向向量。</returns>
    public Vector2 GetMoveDirection()
    {
        // 第一步：读取两个方向的输入值。
        // horizontal：当前横向输入值。
        float horizontal = Input.GetAxisRaw("Horizontal");

        // vertical：当前纵向输入值。
        float vertical = Input.GetAxisRaw("Vertical");

        // 第二步：组合并返回二维方向。
        return new Vector2(horizontal, vertical);
    }

    /// <summary>
    /// 查询左 Shift 是否被按住。
    /// </summary>
    /// <returns>左 Shift 被按住时为 true。</returns>
    public bool IsRunPressed()
    {
        // isRunPressed：左 Shift 是否处于按住状态。
        bool isRunPressed = Input.GetKey(KeyCode.LeftShift);
        return isRunPressed;
    }

    /// <summary>
    /// 查询本帧是否刚按下 E 键。
    /// </summary>
    /// <returns>本帧刚按下 E 时为 true。</returns>
    public bool IsInteractPressed()
    {
        // isInteractPressed：本帧是否刚按下 E 键。
        bool isInteractPressed = Input.GetKeyDown(KeyCode.E);
        return isInteractPressed;
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
