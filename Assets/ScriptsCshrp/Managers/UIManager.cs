using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private ResourceManager _resourceManager;
    private GameObject _inventoryUI;
    private GameObject _settingsUI;

    //自动执行
    public void Initialize(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
        Debug.Log("UIManager 已经接收到 ResourceManager");
    }

    //切换背包状态
    public void ToggleInventory()
    {
        if (_inventoryUI == null || !_inventoryUI.activeSelf)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }
    //打开背包
    public void OpenInventory()
    {
        if (_inventoryUI == null)
        {
            _inventoryUI = _resourceManager.InstantiatePrefab("UI/InventoryUI");
        }

        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(true);
        }
    }
    //关闭背包
    public void CloseInventory()
    {
        if (_inventoryUI != null)
        {
            _inventoryUI.SetActive(false);
        }
    }
    
    //切换设置界面状态
    public void ToggleSettings()
    {
        if (_settingsUI == null || !_settingsUI.activeSelf)
        {
            OpenSettings();
        }
        else
        {
            CloseSettings();
        }
    }

    //打开设置界面
    public void OpenSettings()
    {
        if (_settingsUI == null)
        {
            _settingsUI = _resourceManager.InstantiatePrefab("UI/SettingsUI");
        }
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(true);
        }
    }

    //关闭设置界面
    private void CloseSettings()
    {
        if (_settingsUI != null)
        {
            _settingsUI.SetActive(false);
        }
    }
    
    private void Awake()
    {
        Debug.Log("UIManager: 初始化完成");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
