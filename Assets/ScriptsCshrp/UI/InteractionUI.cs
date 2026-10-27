using UnityEngine;
using TMPro;

/// <summary>
/// 接收文字并更新交互提示的文字组件。
/// </summary>
public class InteractionUI : MonoBehaviour
{
    // ==================== 字段 ====================

    // _contentText：预制体中用于显示交互提示的文字组件。
    [SerializeField] private TMP_Text _contentText;

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 设置交互提示界面显示的文字。
    /// </summary>
    /// <param name="text">需要显示的交互提示文字。</param>
    public void SetText(string text)
    {
        _contentText.text = text;
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
