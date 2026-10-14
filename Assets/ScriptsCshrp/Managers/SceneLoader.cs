using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景加载器。
/// 统一负责游戏中的场景加载与切换。
/// </summary>
public class SceneLoader : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}
