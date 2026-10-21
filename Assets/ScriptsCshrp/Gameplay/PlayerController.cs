using UnityEngine;

/// <summary>
/// 控制玩家移动，查找并交互附近的目标。
/// </summary>
public class PlayerController : MonoBehaviour
{
    // ==================== 字段 ====================

    // _moveSpeed：走路速度，单位为世界单位/秒。
    [SerializeField] private float _moveSpeed = 3f;

    // _runSpeed：跑步速度，单位为世界单位/秒。
    [SerializeField] private float _runSpeed = 5f;

    // _interactRadius：以玩家为中心的交互检测半径。
    [SerializeField] private float _interactRadius = 1.5f;

    // _moveDirection：保存本帧读取的移动方向。
    [SerializeField] private Vector2 _moveDirection;

    // _inputManager：输入管理器组件，由场景入口传入。
    private InPutManager _inputManager;

    // _rigidbody：玩家自身的二维刚体组件，用于移动。
    private Rigidbody2D _rigidbody;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 获取并保存玩家自身的二维刚体组件。
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 输出玩家启动日志。
    /// </summary>
    private void Start()
    {
        Debug.Log("玩家控制脚本已启动");
    }

    /// <summary>
    /// 读取移动方向，响应交互键。
    /// </summary>
    private void Update()
    {
        // 第一步：读取并保存移动方向。
        _moveDirection = _inputManager.GetMoveDirection();

        // 第二步：按下交互键时查找目标。
        if (_inputManager.IsInteractPressed())
        {
            TryInteract();
        }
    }

    /// <summary>
    /// 在物理时间步中执行移动。
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存玩家使用的输入管理器。
    /// </summary>
    /// <param name="inputManager">用于查询玩家输入的输入管理器。</param>
    public void Initialize(InPutManager inputManager)
    {
        _inputManager = inputManager;
    }

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 根据方向和奔跑状态设置刚体速度。
    /// </summary>
    private void Move()
    {
        // 第一步：使用走路速度。
        // currentSpeed：本次移动使用的速度。
        float currentSpeed = _moveSpeed;

        // 第二步：按住奔跑键时改用跑步速度。
        if (_inputManager.IsRunPressed())
        {
            currentSpeed = _runSpeed;
        }

        // 第三步：统一方向长度，设置刚体速度。
        _rigidbody.velocity = _moveDirection.normalized * currentSpeed;
    }

    /// <summary>
    /// 与附近位置最近的可交互物体交互。
    /// </summary>
    private void TryInteract()
    {
        // 第一步：检测附近的碰撞体。
        // colliders：碰撞体数组，保存附近的检测结果。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactRadius);

        Debug.Log("检测到的碰撞体数量：" + colliders.Length);

        // 第二步：准备最近目标的记录。
        // closestInteractable：最近目标的交互组件，初始为空。
        InteractableObject closestInteractable = null;

        // closestDistance：当前最小距离，初始设为最大值。
        float closestDistance = float.MaxValue;

        // 第三步：逐个比较，记录最近的目标。
        // collider：本次从数组中取出的碰撞体。
        foreach (Collider2D collider in colliders)
        {
            // interactableObject：当前物体的交互组件，找不到时为空。
            InteractableObject interactableObject = collider.GetComponent<InteractableObject>();

            // 有交互组件时，才计算距离。
            if (interactableObject != null)
            {
                // distance：玩家到当前目标的位置距离。
                float distance = Vector2.Distance(transform.position,interactableObject.transform.position);

                Debug.Log("目标：" + interactableObject.gameObject.name + "，距离：" + distance);

                // 当前目标更近时，同时更新距离和目标。
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactableObject;
                }
            }
        }

        // 第四步：找到目标后，执行一次交互。
        if (closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

    /// <summary>
    /// 选中玩家时绘制交互检测范围。
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _interactRadius);
    }

}
