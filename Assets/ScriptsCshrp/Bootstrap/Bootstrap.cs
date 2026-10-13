using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏启动入口
/// </summary>
public class Bootstrap : MonoBehaviour
{
    /// <summary>
    /// 当前有效的Bootstrap实例引用
    /// 所有Bootstrap实例共享此静态字段
    /// </summary>
    public static Bootstrap Instance;
    
    //声明字段
    private UIManager _uiManager;
    private ResourceManager _resourceManager;
    private InPutManager _inputManager;

    /// <summary>
    /// 游戏框架启动入口。
    /// 检查并阻止重复的 Bootstrap，
    /// 保存当前实例，并设置整个管理器层级跨场景保留。
    /// 随后依次创建、初始化管理器并启动游戏。 
    /// </summary>
    void Awake()
    {
        //已经存在其他Bootstrap,销毁当前重复对象
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //保存当前Bootstrap的实例引用
        Instance = this;
        DontDestroyOnLoad(gameObject);//设置 Bootstrap 以及子物体跨场景保留
        
        Debug.Log("Bootstrap 启动");
        CreateManagers();
        InitializeManagers();
        StartGame();
    }
    
    /// <summary>
    /// 创建所需管理器
    /// 保存对应的组件引用
    /// </summary>
    private void CreateManagers()
    {
        //创建ResourceManager
        _resourceManager = CreateManager<ResourceManager>("ResourceManager");
        
        //创建UIManager
        _uiManager = CreateManager<UIManager>("UIManager");
        
        //创建InputManager
        _inputManager = CreateManager<InPutManager>("InputManager");
    }

    /// <summary>
    /// 初始化管理器
    /// </summary>
    private void InitializeManagers()
    {
        _uiManager.Initialize(_resourceManager);
        _inputManager.Initialize(_uiManager);
    }
    
    /// <summary>
    /// 执行当前的游戏逻辑
    /// </summary>
    private void StartGame()
    {
        _uiManager.OpenInventory();
        
        GameObject testCube = _resourceManager.InstantiatePrefab("prefabs/TestCube");
        if (testCube != null)
        { 
            testCube.SetActive(false);
        }
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
}
