using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ==================== 字段 ====================

    private InPutManager _inputManager;
    
    // 当前读取到的移动方向，显示在 Inspector 中便于观察。
    [SerializeField] private Vector2 _moveDirection;
    
    // 玩家每秒移动的世界单位数。
    [SerializeField] private float _moveSpeed = 3f;
        
    [SerializeField] private float _runSpeed = 5f;
    
    // ==================== 初始化 ====================

    /// <summary>
    /// 保存输入管理器引用，供玩家读取移动方向。
    /// </summary>
    /// <param name="inputManager">项目中已有的输入管理器。</param>
    public void Initialize(InPutManager inputManager)
    {
        _inputManager = inputManager;
    }
    
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 玩家实例开始运行时，输出启动日志。
    /// </summary>
    private void Start()
    {
        Debug.Log("玩家控制脚本已启动");
    }
    
    /// <summary>
    /// 每帧读取移动方向，再调用移动方法更新玩家位置。
    /// </summary>
    private void Update()
    {
        _moveDirection = _inputManager.GetMoveDirection();
        
        Move();
    }
    
    // ==================== 玩家移动 ====================

    /// <summary>
    /// 根据当前方向和奔跑键状态选择速度，并更新玩家位置。
    /// </summary>
    private void Move()
    {
        // 本次移动默认使用走路速度。
        float currentSpeed = _moveSpeed;

        // 按住奔跑键时，本次移动改用跑步速度。
        if (_inputManager.IsRunPressed())
        {
            currentSpeed = _runSpeed;
        }
        
        // 将二维输入方向转换为世界坐标中的方向，Z 方向保持为零。
        Vector3 direction = new Vector3(_moveDirection.x,_moveDirection.y,0f);
        
        // 根据方向、速度和本帧时间计算位移，并更新玩家位置。
        transform.position += direction.normalized * currentSpeed * Time.deltaTime;
    }
}
