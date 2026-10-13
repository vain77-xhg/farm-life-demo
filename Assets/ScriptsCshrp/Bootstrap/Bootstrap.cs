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
        GameObject resourceObject = new GameObject("ResourceManager");
        resourceObject.transform.SetParent(transform);
        _resourceManager = resourceObject.AddComponent<ResourceManager>();
        
        //创建UIManager
        GameObject uiObject = new GameObject("UIManager");
        uiObject.transform.SetParent(transform);    
        _uiManager = uiObject.AddComponent<UIManager>();
        
        //创建InputManager
        GameObject inputObject = new GameObject("InputManager");
        inputObject.transform.SetParent(transform);
        _inputManager = inputObject.AddComponent<InPutManager>();
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
}
