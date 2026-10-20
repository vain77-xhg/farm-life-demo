using UnityEngine;

/// <summary>
/// 农场场景入口。
/// 组织玩家的创建与初始化，并把玩家的 Transform 传给场景相机。
/// </summary>
public class FarmSceneController : MonoBehaviour
{
    // ==================== 字段 ====================

    // 场景配置：在 FarmSceneRoot 的 Inspector 中绑定 Main Camera 上的组件。
    [SerializeField] private CameraFollow _cameraFollow;

    // 运行时引用：从已经启动的 Bootstrap 获取。
    private ResourceManager _resourceManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 检查游戏启动入口，获取资源管理器并创建玩家。
    /// 直接从农场场景运行且没有 Bootstrap 时，输出提示并结束初始化。
    /// </summary>
    private void Start()
    {
        if (Bootstrap.Instance == null)
        {
            Debug.Log("请从 SampleScene 启动游戏。");
            return;
        }

        _resourceManager = Bootstrap.Instance.GetResourceManager();
        CreatePlayer();
    }

    // ==================== 场景初始化 ====================

    /// <summary>
    /// 加载玩家预制体，将实例归入农场层级并初始化玩家控制组件。
    /// 随后把玩家的 Transform 交给相机，作为跟随目标。
    /// </summary>
    private void CreatePlayer()
    {
        GameObject player = _resourceManager.InstantiatePrefab("Prefabs/Player");
        if (player == null)
        {
            return;
        }

        player.transform.SetParent(transform, false);

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.Log("Player 预制体没有挂载 PlayerController。");
            Destroy(player);
            return;
        }

        // 玩家接收已有的输入管理器，相机接收新玩家的位置组件。
        playerController.Initialize(Bootstrap.Instance.GetInputManager());

        if (_cameraFollow == null)
        {
            Debug.LogError("请在 FarmSceneRoot 上绑定相机跟随组件。");
            return;
        }

        _cameraFollow.Initialize(player.transform);
    }
}
