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
    /// 加载指定名称的场景。
    /// </summary>
    /// <param name="sceneName">需要加载的场景名称。</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
