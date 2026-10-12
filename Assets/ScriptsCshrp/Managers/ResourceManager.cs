using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("ResourceManager 初始化完成");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 寻找到预制体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject LoadPrefab(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError("Prefab 加载失败：" + path);
            return null;
        }
        
        Debug.Log("prefab 加载成功：" + path);
        return prefab;
    }

    /// <summary>
    /// 实例化预制体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject InstantiatePrefab(string path)
    {
        GameObject prefab = LoadPrefab(path);
        if (prefab == null)
        {
            return null;
        }
        GameObject instance = Instantiate(prefab);
        
        return instance;
    }
    
}
