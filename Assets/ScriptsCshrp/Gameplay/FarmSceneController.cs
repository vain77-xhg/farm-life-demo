using UnityEngine;

/// <summary>
/// 农场场景入口。
/// 负责在场景开始时组织农场的初始化流程。
/// </summary>
public class FarmSceneController : MonoBehaviour
{
    // ==================== 字段 ====================
    
    private ResourceManager _resourceManager;
    
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 农场场景开始时执行入口逻辑。
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
    /// 实例化玩家预制体，并将玩家归入当前农场层级。
    /// </summary>
    private void CreatePlayer()
    {
        GameObject player = _resourceManager.InstantiatePrefab("Prefabs/Player");
        if (player == null)
        {
            return;
        }
        player.transform.SetParent(transform,false);

        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.Log("Player 预制体没有挂载 PlayerController。");
            Destroy(player);
            return;
        }
        
        playerController.Initialize(Bootstrap.Instance.GetInputManager());
    }
}
