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

    // _isloading：是否已有加载任务，完成前不接受其他加载请求。
    private bool _isloading = false;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 取消场景加载事件的订阅。
    /// </summary>
    private void OnDestroy()
    {
        // 第一步：没有保存事件管理器时结束。
        if (_eventManager == null)
        {
            return;
        }

        // 第二步：移除初始化时登记的同一个场景加载回调。
        _eventManager.UnsubscribeString(GameEvents.LoadScene, LoadScene);
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存事件管理器并订阅场景加载请求。
    /// </summary>
    /// <param name="eventManager">提供场景加载 string 事件的事件管理器。</param>
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
        // 第一步：记录请求，已有加载任务时忽略本次请求。
        Debug.Log("收到场景加载请求：" + sceneName);

        if (_isloading)
        {
            return;
        }

        // 第二步：检查场景名及 Build Settings 是否允许加载。
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
        // operation：引擎返回的异步任务，通过 progress 和 isDone 查询状态。
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 第二步：任务未完成时输出进度，再等待下一帧检查。
        while (!operation.isDone)
        {
            Debug.Log("场景加载进度：" + operation.progress);

            // 只暂停当前协程，下一帧从这里继续；游戏其他逻辑仍执行。
            yield return null;
        }

        // 第三步：任务完成后退出循环，允许后续加载请求。
        _isloading = false;
        Debug.Log("场景加载完成：" + operation.progress);
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
