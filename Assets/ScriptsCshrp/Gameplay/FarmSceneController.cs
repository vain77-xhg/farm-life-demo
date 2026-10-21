using UnityEngine;

/// <summary>
/// 创建农场玩家并设置相机跟随目标。
/// </summary>
public class FarmSceneController : MonoBehaviour
{
    // ==================== 字段 ====================

    // _cameraFollow：场景相机的跟随组件，在 Inspector 中绑定。
    [SerializeField] private CameraFollow _cameraFollow;

    // _resourceManager：资源管理器组件，用于创建玩家。
    private ResourceManager _resourceManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 获取资源管理器并创建玩家。
    /// </summary>
    private void Start()
    {
        // 第一步：确认游戏启动入口存在。
        if (Bootstrap.Instance == null)
        {
            Debug.Log("请从 SampleScene 启动游戏。");
            return;
        }

        // 第二步：获取资源管理器。
        _resourceManager = Bootstrap.Instance.GetResourceManager();

        // 第三步：创建玩家。
        CreatePlayer();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 创建并初始化玩家，设置相机跟随目标。
    /// </summary>
    private void CreatePlayer()
    {
        // 第一步：创建玩家，失败时结束。
        // player：新创建的玩家实例。
        GameObject player = _resourceManager.InstantiatePrefab("Prefabs/Player");
        if (player == null)
        {
            return;
        }

        // 第二步：设置父对象，获取并检查玩家组件。
        player.transform.SetParent(transform, false);

        // playerController：玩家实例上的控制组件。
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Player 预制体没有挂载 PlayerController。");
            Destroy(player);
            return;
        }

        // 第三步：把输入管理器传给玩家。
        playerController.Initialize(Bootstrap.Instance.GetInputManager());

        // 第四步：检查相机组件，设置跟随目标。
        if (_cameraFollow == null)
        {
            Debug.LogError("请在 FarmSceneRoot 上绑定相机跟随组件。");
            return;
        }

        _cameraFollow.Initialize(player.transform);
    }

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
