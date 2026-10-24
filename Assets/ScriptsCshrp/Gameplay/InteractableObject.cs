using UnityEngine;

/// <summary>
/// 响应玩家交互，输出配置的提示文字。
/// </summary>
public class InteractableObject : MonoBehaviour
{
    // ==================== 字段 ====================

    // _textId：当前物体交互时使用的文本编号。
    [SerializeField] private int _textId;

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 查询并输出当前物体的交互提示。
    /// </summary>
    public void Interact()
    {
        // textManager：启动时创建的文本管理器。
        TextManager textManager = Bootstrap.Instance.GetTextManager();
        
        // text：当前物体对应的交互提示文字。
        string text = textManager.GetText(_textId);
        
        // 没有取得文字时，结束本次交互。
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        
        Debug.Log(text);
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
