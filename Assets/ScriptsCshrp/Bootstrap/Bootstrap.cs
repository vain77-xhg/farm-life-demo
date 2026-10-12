using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏启动入口
/// </summary>
public class Bootstrap : MonoBehaviour
{
    //申明字段
    private UIManager _uiManager;
    private ResourceManager _resourceManager;
    private InPutManager _inputManager;

    void Awake()
    {
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
        _resourceManager = resourceObject.AddComponent<ResourceManager>();
        
        //创建UIManager
        GameObject uiObject = new GameObject("UIManager");
        _uiManager = uiObject.AddComponent<UIManager>();
        
        //创建InputManager
        GameObject inputObject = new GameObject("InputManager");
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
