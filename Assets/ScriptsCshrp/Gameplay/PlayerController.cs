using UnityEngine;

/// <summary>
/// 玩家控制组件。
/// 读取输入，通过二维刚体控制走路和跑步，并检测附近可交互物体。
/// 按下交互键后调用目标组件的 Interact，当前目标通过日志反馈交互结果。
/// </summary>
public class PlayerController : MonoBehaviour
{
    // ==================== 字段 ====================

    // Inspector 配置：走路与跑步时每秒移动的世界单位数。
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _runSpeed = 5f;

    // Inspector 配置：以玩家为中心的交互检测半径。
    [SerializeField] private float _interactRadius = 1.5f;

    // 运行时状态：Update 读取的原始输入方向，在 Inspector 中便于观察。
    [SerializeField] private Vector2 _moveDirection;

    // 运行时引用：输入管理器由场景入口传入，刚体从玩家自身获取。
    private InPutManager _inputManager;
    private Rigidbody2D _rigidbody;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 获取并缓存玩家自身的二维刚体组件。
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 玩家实例开始运行时，输出启动日志。
    /// </summary>
    private void Start()
    {
        Debug.Log("玩家控制脚本已启动");
    }

    /// <summary>
    /// 每帧读取移动方向，并在交互键刚按下时发起一次交互。
    /// </summary>
    private void Update()
    {
        _moveDirection = _inputManager.GetMoveDirection();

        if (_inputManager.IsInteractPressed())
        {
            TryInteract();
        }
    }

    /// <summary>
    /// 在每个物理时间步调用移动方法，设置玩家刚体速度。
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存输入管理器引用，供玩家读取移动、奔跑和交互输入。
    /// </summary>
    /// <param name="inputManager">Bootstrap 已经创建的输入管理器。</param>
    public void Initialize(InPutManager inputManager)
    {
        _inputManager = inputManager;
    }

    // ==================== 玩家移动 ====================

    /// <summary>
    /// 根据已保存的方向和当前奔跑键状态，设置玩家刚体的移动速度。
    /// 无方向输入时写入零速度，使玩家停止。
    /// </summary>
    private void Move()
    {
        // 每次先选择走路速度，松开奔跑键后会自然恢复走路。
        float currentSpeed = _moveSpeed;

        if (_inputManager.IsRunPressed())
        {
            currentSpeed = _runSpeed;
        }

        // normalized 统一斜向速度；velocity 已是每秒速率，无需再乘帧时间。
        _rigidbody.velocity = _moveDirection.normalized * currentSpeed;
    }

    // ==================== 玩家交互 ====================

    /// <summary>
    /// 检测玩家附近的二维碰撞体，输出检测数量，
    /// 并与遍历时找到的第一个可交互物体执行一次交互。
    /// </summary>
    private void TryInteract()
    {
        // 以玩家为圆心查询附近的碰撞体，结果中也可能包含玩家自身。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _interactRadius);
        Debug.Log("检测到的碰撞体数量：" + colliders.Length);

        // 逐个检查检测结果，从碰撞体所在的同一个 GameObject 上获取交互组件。
        foreach (Collider2D collider in colliders)
        {
            InteractableObject interactableObject = collider.GetComponent<InteractableObject>();

            // 找到交互组件才调用；没有组件时，循环会继续检查下一个碰撞体。
            if (interactableObject != null)
            {
                interactableObject.Interact();

                // 结束整个方法，使一次按键只交互一个物体；当前不按距离选择目标。
                return;
            }
        }
    }
    
    // =================== 调试显示 =======================
    
    /// <summary>
    /// 选中玩家时，绘制交互检测范围，方便观察和调整半径。
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _interactRadius);
    }
}
