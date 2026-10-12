using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InPutManager : MonoBehaviour
{
    private UIManager _uiManager;
    public void Initialize(UIManager uiManager)
    {
        _uiManager = uiManager;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("检测到Tab按键按下");
            _uiManager.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("检测到ESC键按下");
            _uiManager.ToggleSettings();
        }

        
        
        
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("检测到鼠标左键按下");
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("鼠标点击发生在UI上");
                return;
            }
            else
            {
                Debug.Log("鼠标点击发生在UI外");
                //处理游戏世界的点击
            }
        }
    }

    
}
