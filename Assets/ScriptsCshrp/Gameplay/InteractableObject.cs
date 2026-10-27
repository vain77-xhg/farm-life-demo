using UnityEngine;

/// <summary>
/// 响应玩家交互，请求显示配置的提示文字。
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
    /// 查询交互文字，并发布显示请求。
    /// </summary>
    public void Interact()
    {
        // 第一步：查询当前物体对应的文字。
        // textManager：启动时创建的文本管理器。
        TextManager textManager = Bootstrap.Instance.GetTextManager();

        // text：当前物体对应的交互提示文字。
        string text = textManager.GetText(_textId);

        // 第二步：没有文字时结束本次交互。
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        // 第三步：取得事件管理器，发布显示请求。
        // eventManager：用于发布交互提示显示请求的事件管理器。
        EventManager eventManager = Bootstrap.Instance.GetEventManager();

        eventManager.PublishString(GameEvents.ShowInteractionText, text);
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
