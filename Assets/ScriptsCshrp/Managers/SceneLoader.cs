using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 接收场景请求并异步加载目标场景。
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // ==================== 字段 ====================

    // _eventManager：事件管理器组件，用于订阅场景加载请求。
    private EventManager _eventManager;

    // _isloading：是否正在加载场景，用于阻止重复加载。
    private bool _isloading = false;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 取消场景加载事件的订阅。
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
    /// 保存事件管理器并订阅场景加载请求。
    /// </summary>
    /// <param name="eventManager">用于发布或订阅事件的事件管理器。</param>
    public void Initialize(EventManager eventManager)
    {
        // 第一步：保存事件管理器。
        _eventManager = eventManager;

        // 第二步：订阅带场景名称的加载事件。
        _eventManager.SubscribeString(GameEvents.LoadScene, LoadScene);
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 检查场景请求并启动加载协程。
    /// </summary>
    /// <param name="sceneName">需要加载的场景名称。</param>
    public void LoadScene(string sceneName)
    {
        // 第一步：记录请求，正在加载时结束。
        Debug.Log("收到场景加载请求：" + sceneName);

        if (_isloading)
        {
            return;
        }

        // 第二步：确认目标场景可以加载。
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("无法加载场景，请检查场景名称和 Build Settings：" + sceneName);
            return;
        }

        // 第三步：标记加载状态，启动协程。
        _isloading = true;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 等待异步加载完成并恢复加载状态。
    /// </summary>
    /// <param name="sceneName">需要加载的场景名称。</param>
    /// <returns>供 StartCoroutine 执行的协程迭代器。</returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // 第一步：启动异步场景加载。
        // operation：保存场景加载进度和完成状态的异步操作。
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 第二步：加载期间逐帧输出进度。
        while (!operation.isDone)
        {
            Debug.Log("场景加载进度：" + operation.progress);

            // 暂停当前协程，下一帧继续检查。
            yield return null;
        }

        // 第三步：加载完成后解除加载状态。
        _isloading = false;
        Debug.Log("场景加载完成：" + operation.progress);
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
