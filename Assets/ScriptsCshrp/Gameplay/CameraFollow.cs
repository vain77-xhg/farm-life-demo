using UnityEngine;

/// <summary>
/// 让相机跟随目标的二维位置。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // ==================== 字段 ====================

    // _target：需要跟随的目标 Transform，由农场入口传入。
    private Transform _target;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 跟随目标的 X、Y 坐标，保留相机的 Z 坐标。
    /// </summary>
    private void LateUpdate()
    {
        // 第一步：没有目标时结束本次跟随。
        if (_target == null)
        {
            return;
        }

        // 第二步：更新相机位置。
        transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存相机的跟随目标。
    /// </summary>
    /// <param name="target">相机需要跟随的目标 Transform。</param>
    public void Initialize(Transform target)
    {
        _target = target;
    }

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
