using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景加载器。
/// 响应场景加载事件，通过协程等待异步加载完成，并防止加载期间重复发起请求。
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // ==================== 字段 ====================

    // 由 Bootstrap 传入，用于订阅和取消订阅场景加载请求。
    private EventManager _eventManager;

    // 当前是否正在加载场景，正常加载完成后恢复为 false。
    private bool _isloading = false;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 组件销毁时取消场景加载事件订阅。
    /// </summary>
    private void OnDestroy()
    {
        if (_eventManager == null)
        {
            return;
        }

        _eventManager.UnsubscribeString(GameEvents.LoadScene, LoadScene);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存事件管理器引用，并订阅带场景名称的加载请求。
    /// </summary>
    /// <param name="eventManager">Bootstrap 已经创建的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
        _eventManager.SubscribeString(GameEvents.LoadScene, LoadScene);
    }

    // ==================== 场景加载 ====================

    /// <summary>
    /// 检查请求后启动异步加载协程。
    /// 正在加载时忽略新请求；目标场景不可加载时输出错误并结束本次请求。
    /// </summary>
    /// <param name="sceneName">目标场景名称，需要已加入并启用在 Build Settings 中。</param>
    public void LoadScene(string sceneName)
    {
        Debug.Log("收到场景加载请求：" + sceneName);

        if (_isloading)
        {
            return;
        }

        // 在设置加载状态之前检查场景，避免无效请求占用加载状态。
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("无法加载场景，请检查场景名称和 Build Settings：" + sceneName);
            return;
        }

        _isloading = true;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// 发起异步场景加载，在等待期间逐帧输出进度。
    /// 加载完成后解除加载状态，并输出完成日志。
    /// </summary>
    /// <param name="sceneName">需要加载的场景名称。</param>
    /// <returns>交给 StartCoroutine 执行的协程迭代器。</returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            Debug.Log("场景加载进度：" + operation.progress);

            // 暂停当前协程，下一帧继续检查；游戏的其他逻辑仍可运行。
            yield return null;
        }

        _isloading = false;
        Debug.Log("场景加载完成：" + operation.progress);
    }
}
