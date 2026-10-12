using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Start is called before the first frame update

    void Awake()
    {
        Debug.Log("Bootstrap 启动");
        //创建ResourceManager
        GameObject resourceObject = new GameObject("ResourceManager");
        ResourceManager resourceManager = resourceObject.AddComponent<ResourceManager>();
        
        //创建UIManager
        GameObject uiObject = new GameObject("UIManager");
        UIManager uiManager = uiObject.AddComponent<UIManager>();
        uiManager.Initialize(resourceManager);
        
        //创建InputManager
        GameObject inputObject = new GameObject("InputManager");
        InPutManager inputManager = inputObject.AddComponent<InPutManager>();
        inputManager.Initialize(uiManager);
        
        uiManager.OpenInventory();

        GameObject testCube = resourceManager.InstantiatePrefab("prefabs/TestCube");
        if (testCube != null)
        {
            testCube.SetActive(false);
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
