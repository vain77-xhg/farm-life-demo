using UnityEngine;

/// <summary>
/// 资源管理器。
/// 统一提供 Resources 预制体加载与实例化入口。
/// </summary>
public class ResourceManager : MonoBehaviour
{
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 资源管理器组件创建时，输出当前的启动日志。
    /// </summary>
    void Awake()
    {
        Debug.Log("ResourceManager 初始化完成");
    }

    // ==================== 预制体加载 ====================

    /// <summary>
    /// 从 Resources 目录加载预制体资源；加载失败时输出错误并返回 null。
    /// 此方法返回资源引用，实例的创建由 InstantiatePrefab 负责。
    /// </summary>
    /// <param name="path">相对于 Resources 目录的路径，不包含 Resources 前缀与文件扩展名。</param>
    /// <returns>加载到的预制体资源；失败时返回 null。</returns>
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

    // ==================== 预制体实例化 ====================

    /// <summary>
    /// 加载指定预制体并创建一个实例，供调用方安排父对象、初始化和显示状态。
    /// </summary>
    /// <param name="path">相对于 Resources 目录的路径，不包含文件扩展名。</param>
    /// <returns>新创建的实例；资源加载失败时返回 null。</returns>
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
