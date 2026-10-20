using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 输入管理
/// </summary>
public class InPutManager : MonoBehaviour
{
    // ==================== 字段 ====================

    private EventManager _eventManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 每帧检测玩家输入。
    /// Tab 发布背包切换事件，Escape 发布设置切换事件；
    /// 鼠标左键点击时检查指针是否位于 UI 上。
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
                // 处理游戏世界的点击
            }
        }
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 初始化输入管理器。
    /// 保存事件管理器引用，用于发布玩家输入事件。
    /// </summary>
    /// <param name="eventManager">事件管理器引用。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }
    
    // ==================== 移动输入 ====================

    /// <summary>
    /// 读取 WASD 或方向键，返回当前的原始移动方向。
    /// </summary>
    /// <returns>横向和纵向组成的方向向量，无输入时为零向量。</returns>
    public Vector2 GetMoveDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        return new Vector2(horizontal, vertical);
    }

    /// <summary>
    /// 检测玩家是否按住左 Shift 奔跑键。
    /// </summary>
    /// <returns>按住时返回 true，否则返回 false。</returns>
    public bool IsRunPressed()
    {
        bool isRunPressed = Input.GetKey(KeyCode.LeftShift);
        return isRunPressed;
    }
}
