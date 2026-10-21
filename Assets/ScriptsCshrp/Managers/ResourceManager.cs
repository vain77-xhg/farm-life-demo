using UnityEngine;

/// <summary>
/// 从 Resources 加载预制体并创建实例。
/// </summary>
public class ResourceManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 输出资源管理器启动日志。
    /// </summary>
    void Awake()
    {
        Debug.Log("ResourceManager 初始化完成");
    }

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 加载指定路径的预制体资源。
    /// </summary>
    /// <param name="path">Resources 下的预制体路径，不含扩展名。</param>
    /// <returns>加载到的预制体资源，失败时为空。</returns>
    public GameObject LoadPrefab(string path)
    {
        // 第一步：加载预制体资源。
        // prefab：加载到的预制体资源。
        GameObject prefab = Resources.Load<GameObject>(path);

        // 第二步：加载失败时返回空值。
        if (prefab == null)
        {
            Debug.LogError("Prefab 加载失败：" + path);
            return null;
        }

        // 第三步：输出成功日志并返回资源。
        Debug.Log("prefab 加载成功：" + path);
        return prefab;
    }

    /// <summary>
    /// 加载预制体并创建一个实例。
    /// </summary>
    /// <param name="path">Resources 下的预制体路径，不含扩展名。</param>
    /// <returns>新创建的实例，加载失败时为空。</returns>
    public GameObject InstantiatePrefab(string path)
    {
        // 第一步：加载资源，失败时结束。
        // prefab：加载到的预制体资源。
        GameObject prefab = LoadPrefab(path);
        if (prefab == null)
        {
            return null;
        }

        // 第二步：创建并返回实例。
        // instance：通过预制体创建的新实例。
        GameObject instance = Instantiate(prefab);
        return instance;
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
