using UnityEngine;

/// <summary>
/// 游戏启动入口。
/// 保留唯一的 Bootstrap，创建并初始化管理器，然后进入农场场景。
/// </summary>
public class Bootstrap : MonoBehaviour
{
    // ==================== 字段 ====================

    /// <summary>
    /// 当前有效的 Bootstrap 实例，供场景入口访问已经创建的管理器。
    /// </summary>
    public static Bootstrap Instance;

    // 运行时创建的管理器组件，所在对象均为 Bootstrap 的子对象。
    private UIManager _uiManager;
    private ResourceManager _resourceManager;
    private InPutManager _inputManager;
    private EventManager _eventManager;
    private SceneLoader _sceneLoader;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 阻止重复的启动入口，将当前对象及其子对象跨场景保留。
    /// 随后依次创建管理器、传入依赖并执行游戏启动流程。
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("Bootstrap 启动");
        CreateManagers();
        InitializeManagers();
        StartGame();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 依次创建资源、界面、输入、事件和场景加载管理器，保存组件引用。
    /// 所有管理器创建完成后，再统一传入它们需要的依赖。
    /// </summary>
    private void CreateManagers()
    {
        _resourceManager = CreateManager<ResourceManager>("ResourceManager");
        _uiManager = CreateManager<UIManager>("UIManager");
        _inputManager = CreateManager<InPutManager>("InputManager");
        _eventManager = CreateManager<EventManager>("EventManager");
        _sceneLoader = CreateManager<SceneLoader>("SceneLoader");
    }

    /// <summary>
    /// 为界面管理器传入资源和事件管理器，为输入管理器及场景加载器传入事件管理器。
    /// 各组件在自己的 Initialize 方法中保存引用并完成相应的事件订阅。
    /// </summary>
    private void InitializeManagers()
    {
        _uiManager.Initialize(_resourceManager, _eventManager);
        _inputManager.Initialize(_eventManager);
        _sceneLoader.Initialize(_eventManager);
    }

    // ==================== 管理器访问 ====================

    /// <summary>
    /// 获取启动时已经创建的资源管理器。
    /// </summary>
    /// <returns>当前 Bootstrap 保存的资源管理器引用。</returns>
    public ResourceManager GetResourceManager()
    {
        return _resourceManager;
    }

    /// <summary>
    /// 获取启动时已经创建的输入管理器。
    /// </summary>
    /// <returns>当前 Bootstrap 保存的输入管理器引用。</returns>
    public InPutManager GetInputManager()
    {
        return _inputManager;
    }

    // ==================== 游戏启动 ====================

    /// <summary>
    /// 执行当前的启动测试流程：打开背包，创建并隐藏测试方块，最后发布农场场景加载请求。
    /// </summary>
    private void StartGame()
    {
        _uiManager.OpenInventory();

        GameObject testCube = _resourceManager.InstantiatePrefab("prefabs/TestCube");
        if (testCube != null)
        {
            testCube.SetActive(false);
        }

        _eventManager.PublishString(GameEvents.LoadScene, GameScenes.FarmScene);
    }

    // ==================== 内部辅助 ====================

    /// <summary>
    /// 创建管理器对象，将其放在 Bootstrap 层级下，再添加指定类型的组件。
    /// </summary>
    /// <typeparam name="T">需要创建的组件类型。</typeparam>
    /// <param name="objectName">管理器 GameObject 的名称。</param>
    /// <returns>新对象上添加的管理器组件。</returns>
    private T CreateManager<T>(string objectName) where T : Component
    {
        GameObject managerObject = new GameObject(objectName);
        managerObject.transform.SetParent(transform);
        T manager = managerObject.AddComponent<T>();
        return manager;
    }
}
