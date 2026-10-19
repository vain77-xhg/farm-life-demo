using UnityEngine;

/// <summary>
/// 农场场景入口。
/// 负责在场景开始时准备农场中的游戏对象。
/// </summary>
public class FarmSceneController : MonoBehaviour
{
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 场景入口组件启用后，在首次 Update 前执行一次。
    /// 当前用于验证农场场景的启动流程。
    /// </summary>
    private void Start()
    {
        Debug.Log("农场场景入口已启动");
    }
}
