using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景加载器。
/// 统一负责游戏中的场景加载与切换。
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // ==================== 字段 ====================

    private EventManager _eventManager;

    private bool _isloading = false;
    
    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// SceneLoader 被销毁时取消场景加载事件订阅。
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
    /// 初始化场景加载器。
    /// 保存事件管理器引用，并订阅场景加载事件。
    /// </summary>
    /// <param name="eventManager">事件管理器引用。</param>
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;

        // 订阅场景加载事件
        _eventManager.SubscribeString(GameEvents.LoadScene, LoadScene);
        
    }
    
    // ==================== 场景加载 ====================
    
    /// <summary>
    /// 请求加载指定场景。
    /// 对外提供统一的场景加载入口。
    /// 正在加载时忽略新的请求。
    /// 目标场景不可加载时输出错误并结束请求。
    /// </summary>
    /// <param name="sceneName">需要加载的场景名称。</param>
    public void LoadScene(string sceneName)
    {
        Debug.Log("收到场景加载请求：" + sceneName);
        if (_isloading)
        {
            return;
        }
        //判断指定场景是否能加载
        if (!Application.CanStreamedLevelBeLoaded(sceneName))//返回的是bool，方法的作用是检查指定场景是否可以加载
        {
            Debug.LogError("无法加载场景，请检查场景名称和 Build Settings："+sceneName);
            return;
        }
        _isloading = true;
        //启动并管理传进来的协程
        StartCoroutine(LoadSceneAsync(sceneName));
        
    }
    
    /// <summary>
    /// 异步加载指定场景。
    /// </summary>
    /// <param name="sceneName">需要加载的场景名称。</param>
    private IEnumerator LoadSceneAsync(string sceneName)// IEnumerator 一种“可以分段执行、可以暂停再继续”的方法返回类型。
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            Debug.Log("场景加载进度：" + operation.progress);
            
            yield return null;
        }
        _isloading = false;
        Debug.Log("场景加载完成：" + operation.progress);
    }
}
