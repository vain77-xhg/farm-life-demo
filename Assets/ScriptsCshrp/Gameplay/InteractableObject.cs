using UnityEngine;

/// <summary>
/// 提供场景物体的交互入口，默认显示配置的文字提示。
/// </summary>
public class InteractableObject : MonoBehaviour
{
    // ==================== 字段 ====================

    // _textId：当前物体对应的文本编号，用于查询提示文字。
    [SerializeField] private int _textId;

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 查询并请求显示交互文字，子类可以重写此方法来执行其他行为。
    /// </summary>
    public virtual void Interact()
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

        // 第三步：取得事件管理器，把查到的文字发布给界面显示。
        // eventManager：用于发布交互提示显示请求的事件管理器。
        EventManager eventManager = Bootstrap.Instance.GetEventManager();

        eventManager.PublishString(GameEvents.ShowInteractionText, text);
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
