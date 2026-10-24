using UnityEngine;

/// <summary>
/// 创建并初始化游戏管理器，启动游戏。
/// </summary>
public class Bootstrap : MonoBehaviour
{
    // ==================== 字段 ====================

    /// <summary>
    /// Instance：当前唯一的游戏启动入口。
    /// </summary>
    public static Bootstrap Instance;

    // _uiManager：界面管理器组件，控制背包和设置界面。
    private UIManager _uiManager;

    // _resourceManager：资源管理器，加载预制体和文本资源，并创建预制体实例。
    private ResourceManager _resourceManager;

    // _inputManager：输入管理器组件，查询输入和发布界面事件。
    private InPutManager _inputManager;

    // _eventManager：事件管理器组件，保存和调用事件回调。
    private EventManager _eventManager;

    // _sceneLoader：场景加载器组件，处理场景切换。
    private SceneLoader _sceneLoader;
    
    // _textManager：文本管理器，读取文本表并按编号提供文字。
    private TextManager _textManager;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 保留唯一启动入口，初始化管理器并启动游戏。
    /// </summary>
    void Awake()
    {
        // 第一步：检查入口，重复时销毁。
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 第二步：保存当前入口，跨场景保留对象。
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("Bootstrap 启动");

        // 第三步：创建管理器，再传入依赖。
        CreateManagers();
        InitializeManagers();

        // 第四步：执行游戏启动流程。
        StartGame();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 创建各管理器并保存组件引用。
    /// </summary>
    private void CreateManagers()
    {
        _resourceManager = CreateManager<ResourceManager>("ResourceManager");
        _uiManager = CreateManager<UIManager>("UIManager");
        _inputManager = CreateManager<InPutManager>("InputManager");
        _eventManager = CreateManager<EventManager>("EventManager");
        _sceneLoader = CreateManager<SceneLoader>("SceneLoader");
        _textManager=CreateManager<TextManager>("TextManager");
    }

    /// <summary>
    /// 向各管理器传入需要的依赖。
    /// </summary>
    private void InitializeManagers()
    {
        _uiManager.Initialize(_resourceManager, _eventManager);
        _inputManager.Initialize(_eventManager);
        _sceneLoader.Initialize(_eventManager);
        _textManager.Initialize(_resourceManager);
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 获取已有的资源管理器。
    /// </summary>
    /// <returns>启动时创建的资源管理器组件。</returns>
    public ResourceManager GetResourceManager()
    {
        return _resourceManager;
    }

    /// <summary>
    /// 获取已有的输入管理器。
    /// </summary>
    /// <returns>启动时创建的输入管理器组件。</returns>
    public InPutManager GetInputManager()
    {
        return _inputManager;
    }

    /// <summary>
    /// 获取已有的文本管理器
    /// </summary>
    /// <returns>启动时创建的文本管理器</returns>
    public TextManager GetTextManager()
    {
        return _textManager;
    }
    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 执行背包、方块和场景切换的启动测试。
    /// </summary>
    private void StartGame()
    {
        // 第一步：打开背包。
        _uiManager.OpenInventory();

        // 第二步：创建测试方块，成功后隐藏。
        // testCube：启动测试时创建的方块实例。
        GameObject testCube = _resourceManager.InstantiatePrefab("prefabs/TestCube");
        if (testCube != null)
        {
            testCube.SetActive(false);
        }

        // 第三步：发布农场场景加载请求。
        _eventManager.PublishString(GameEvents.LoadScene, GameScenes.FarmScene);
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    /// <summary>
    /// 创建管理器子对象并返回添加的组件。
    /// </summary>
    /// <param name="objectName">新建管理器对象的名称。</param>
    /// <typeparam name="T">需要添加的管理器组件类型。</typeparam>
    /// <returns>新对象上添加的管理器组件。</returns>
    private T CreateManager<T>(string objectName) where T : Component
    {
        // 第一步：创建管理器对象。
        // managerObject：新创建的管理器 GameObject。
        GameObject managerObject = new GameObject(objectName);

        // 第二步：设置 Bootstrap 为父对象。
        managerObject.transform.SetParent(transform);

        // 第三步：添加组件并返回。
        // manager：添加到新对象上的管理器组件。
        T manager = managerObject.AddComponent<T>();
        return manager;
    }

    // ==================== 调试显示 ====================

}
