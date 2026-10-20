using UnityEngine;

/// <summary>
/// 相机跟随组件。
/// 跟随目标的 X、Y 世界坐标，并保留相机自身的 Z 坐标。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // ==================== 字段 ====================

    // 运行时引用：场景入口创建玩家后，将玩家的 Transform 传入。
    private Transform _target;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 在本帧 Update 之后对齐目标的 X、Y 位置，保持相机原有拍摄距离。
    /// 目标尚未传入或已经被销毁时，保留当前相机位置。
    /// </summary>
    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存相机需要跟随的目标引用。
    /// 此方法由农场场景入口在玩家创建完成后调用。
    /// </summary>
    /// <param name="target">需要跟随的玩家 Transform。</param>
    public void Initialize(Transform target)
    {
        _target = target;
    }
}
