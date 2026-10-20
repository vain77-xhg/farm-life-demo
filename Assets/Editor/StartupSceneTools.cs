using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 启动场景编辑器工具。
/// 通过菜单打开启动场景，方便从 Bootstrap 入口开始测试。
/// 此脚本放在 Editor 文件夹中，仅供编辑器使用。
/// </summary>
public class StartupSceneTools
{
    // ==================== 字段 ====================

    // 编辑器打开场景时使用的工程相对路径，包含文件扩展名。
    private const string StartupScenePath = "Assets/Scenes/SampleScene.scene";

    // ==================== 菜单入口 ====================

    /// <summary>
    /// 在编辑模式下打开启动场景，不自动开始运行。
    /// 切换前询问是否保存已修改的场景；用户取消时保留当前场景。
    /// </summary>
    [MenuItem("工具/打开启动场景")]
    private static void OpenStartupScene()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("请先停止运行，再打开启动场景。");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        EditorSceneManager.OpenScene(StartupScenePath);
    }
}
