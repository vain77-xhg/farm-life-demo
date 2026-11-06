using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 提供打开启动场景的编辑器菜单。
/// </summary>
public class StartupSceneTools
{
    // ==================== 字段 ====================

    // StartupScenePath：启动场景的工程相对路径，包含扩展名。
    private const string StartupScenePath = "Assets/Scenes/SampleScene.scene";

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 确认场景保存选择后，通过编辑器菜单打开启动场景。
    /// </summary>
    [MenuItem("工具/打开启动场景")]
    private static void OpenStartupScene()
    {
        // 第一步：正在进入或处于运行模式时结束。
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("请先停止运行，再打开启动场景。");
            return;
        }

        // 第二步：让用户选择是否保存当前场景修改，取消时不切换场景。
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        // 第三步：打开启动场景。
        EditorSceneManager.OpenScene(StartupScenePath);
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
