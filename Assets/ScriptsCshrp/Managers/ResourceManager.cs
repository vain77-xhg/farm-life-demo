using UnityEngine;

/// <summary>
/// 资源管理
/// </summary>
public class ResourceManager : MonoBehaviour
{
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 输出资源管理器的初始化日志。
    /// </summary>
    void Awake()
    {
        Debug.Log("ResourceManager 初始化完成");
    }

    // ==================== 预制体加载与实例化 ====================

    /// <summary>
    /// 从 Resources 目录加载指定路径的预制体。
    /// 加载失败时输出错误日志并返回 null。
    /// </summary>
    /// <param name="path">相对于 Resources 目录的资源路径，不包含文件扩展名。</param>
    /// <returns>加载成功的预制体；加载失败时为 null。</returns>
    public GameObject LoadPrefab(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError("Prefab 加载失败：" + path);
            return null;
        }

        Debug.Log("prefab 加载成功：" + path);
        return prefab;
    }

    /// <summary>
    /// 加载并实例化指定路径的预制体。
    /// 加载失败时返回 null。
    /// </summary>
    /// <param name="path">相对于 Resources 目录的资源路径，不包含文件扩展名。</param>
    /// <returns>创建的预制体实例；加载失败时为 null。</returns>
    public GameObject InstantiatePrefab(string path)
    {
        GameObject prefab = LoadPrefab(path);
        if (prefab == null)
        {
            return null;
        }
        GameObject instance = Instantiate(prefab);

        return instance;
    }
}
