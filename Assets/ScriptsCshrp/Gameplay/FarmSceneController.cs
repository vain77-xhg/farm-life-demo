using UnityEngine;

/// <summary>
/// 农场场景入口。
/// 负责在场景开始时组织农场的初始化流程。
/// </summary>
public class FarmSceneController : MonoBehaviour
{
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 农场场景开始时执行入口逻辑。
    /// </summary>
    private void Start()
    {
        Debug.Log("农场场景入口已启动");
    }
}
