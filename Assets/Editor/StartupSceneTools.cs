using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 启动场景编辑器工具。
/// 提供快速打开游戏启动场景的菜单入口。
/// </summary>
public class StartupSceneTools
{
    // ==================== 场景路径 ====================

    private const string StartupScenePath = "Assets/Scenes/SampleScene.scene";

    // ==================== 菜单操作 ====================

    /// <summary>
    /// 在编辑模式下打开启动场景。
    /// 切换前询问是否保存当前场景的修改。
    /// </summary>
    [MenuItem("工具/打开启动场景")]
    private static void OpenStartupScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("请先停止运行，再打开启动场景。");
            return;
        }

        // 如果取消切换，就保留当前场景
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        EditorSceneManager.OpenScene(StartupScenePath);
    }
}
