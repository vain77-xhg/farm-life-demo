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
    /// 逐帧发布界面快捷键事件，检测鼠标是否点击 UI。
    /// </summary>
    void Update()
    {
        // 第一步：按 Tab 时发布背包切换请求，不直接控制界面。
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("检测到Tab按键按下");
            _eventManager.Publish(GameEvents.ToggleInventory);
        }

        // 第二步：按 Escape 时发布设置切换请求。
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("检测到ESC键按下");
            _eventManager.Publish(GameEvents.ToggleSettings);
        }

        // 第三步：借助 EventSystem 判断鼠标是否点在 UI 上并输出日志。
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
    /// <param name="eventManager">接收界面快捷键请求的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 读取横向和纵向输入，返回移动方向。
    /// </summary>
    /// <returns>尚未归一化的二维输入方向，不直接移动玩家。</returns>
    public Vector2 GetMoveDirection()
    {
        // 第一步：读取两个方向的输入值。
        // horizontal：横向输入值，负值向左、正值向右。
        float horizontal = Input.GetAxisRaw("Horizontal");

        // vertical：纵向输入值，负值向下、正值向上。
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
