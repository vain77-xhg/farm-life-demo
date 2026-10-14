using UnityEngine;

/// <summary>
/// 游戏启动入口
/// </summary>
public class Bootstrap : MonoBehaviour
{
    // ==================== 字段 ====================

    /// <summary>
    /// 当前有效的Bootstrap实例引用
    /// 所有Bootstrap实例共享此静态字段
    /// </summary>
    public static Bootstrap Instance;

    private UIManager _uiManager;
    private ResourceManager _resourceManager;
    private InPutManager _inputManager;
    private EventManager _eventManager;
    private SceneLoader _sceneLoader;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 游戏框架启动入口。
    /// 检查并阻止重复的 Bootstrap，
    /// 保存当前实例，并设置整个管理器层级跨场景保留。
    /// 随后依次创建、初始化管理器并启动游戏。
    /// </summary>
    void Awake()
    {
        // 已经存在其他Bootstrap,销毁当前重复对象
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // 保存当前Bootstrap的实例引用
        Instance = this;
        DontDestroyOnLoad(gameObject); // 设置 Bootstrap 以及子物体跨场景保留

        Debug.Log("Bootstrap 启动");
        CreateManagers();
        InitializeManagers();
        StartGame();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 按现有顺序创建资源、界面、输入和事件管理器，
    /// 并保存对应的组件引用。
    /// </summary>
    private void CreateManagers()
    {
        // 创建ResourceManager
        _resourceManager = CreateManager<ResourceManager>("ResourceManager");

        // 创建UIManager
        _uiManager = CreateManager<UIManager>("UIManager");

        // 创建InputManager
        _inputManager = CreateManager<InPutManager>("InputManager");

        // 创建EventManager
        _eventManager = CreateManager<EventManager>("EventManager");
        
        //创建SceneLoader
        _sceneLoader = CreateManager<SceneLoader>("SceneLoader");
    }

    /// <summary>
    /// 为 UIManager 传入资源管理器和事件管理器，
    /// 为 InPutManager 传入事件管理器。
    /// </summary>
    private void InitializeManagers()
    {
        _uiManager.Initialize(_resourceManager, _eventManager);
        _inputManager.Initialize(_eventManager);
        _sceneLoader.Initialize(_eventManager);
    }

    /// <summary>
    /// 创建指定类型的管理器组件，
    /// 将其 GameObject 设置为 Bootstrap 的子物体，
    /// 并返回创建后的组件引用。
    /// </summary>
    /// <typeparam name="T">要创建的管理器组件类型。</typeparam>
    /// <param name="objectName">新建 GameObject 的名称。</param>
    /// <returns>创建完成的管理器组件引用。</returns>
    private T CreateManager<T>(string objectName) where T : Component
    {
        // 创建一个空的 GameObject
        GameObject managerObject = new GameObject(objectName);

        // 将这个 GameObject 设置为 Bootstrap 的子物体
        managerObject.transform.SetParent(transform);

        // 给这个 GameObject 添加 T 类型的组件
        T manager = managerObject.AddComponent<T>();

        // 返回这个组件的引用
        return manager;
    }

    // ==================== 游戏启动 ====================

    /// <summary>
    /// 执行当前的游戏启动逻辑。
    /// 打开背包界面，并创建测试方块；创建成功后隐藏测试方块。
    /// </summary>
    private void StartGame()
    {
        _uiManager.OpenInventory();

        GameObject testCube = _resourceManager.InstantiatePrefab("prefabs/TestCube");
        if (testCube != null)
        {
            testCube.SetActive(false);
        }

        _eventManager.PublishString(GameEvents.LoadScene, GameScenes.TestScene);
    }
}
