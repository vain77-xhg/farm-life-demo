using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置界面组件。
/// 通过代码创建 Canvas、背景面板和关闭按钮，按钮点击行为尚待绑定。
/// </summary>
public class SettingsUI : MonoBehaviour
{
    // ==================== 字段 ====================

    // 保存代码创建的 Button 组件引用，供后续绑定点击行为
    [SerializeField] private Button _closeButton;

    // 保存背景面板对象，创建按钮时用它指定父对象
    private GameObject _settingsPanel;

    // ==================== 初始化 ====================

    /// <summary>
    /// 创建屏幕覆盖模式的 Canvas，配置鼠标命中检测，
    /// 并依次创建背景面板和关闭按钮。
    /// 由 UIManager 在首次创建本组件后调用一次。
    /// </summary>
    public void Initialize()
    {
        // gameObject 指当前脚本所在的设置根对象；添加组件后保存其引用
        Canvas canvas = gameObject.AddComponent<Canvas>();

        // 让 UI 以屏幕覆盖模式绘制在游戏画面上层
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 让当前 Canvas 上的 UI 图形参与 EventSystem 的鼠标命中检测
        gameObject.AddComponent<GraphicRaycaster>();

        // 按钮需要以背景面板为父对象，因此必须先创建面板
        CreatePanel();
        CreateCloseButton();
    }

    // ==================== 界面创建 ====================

    /// <summary>
    /// 在设置根对象下创建背景面板，并配置尺寸和颜色。
    /// </summary>
    private void CreatePanel()
    {
        // 创建面板时同时添加 RectTransform，供后续配置 UI 布局
        _settingsPanel = new GameObject("SettingsPanel", typeof(RectTransform));

        // 设置父对象，并保留局部变换值
        _settingsPanel.transform.SetParent(transform, false);

        // RectTransform 已在创建对象时添加，因此这里直接获取
        RectTransform panelRect = _settingsPanel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(600, 400);

        // Image 负责绘制面板背景；添加后通过返回的引用设置颜色
        Image panelImage = _settingsPanel.AddComponent<Image>();
        panelImage.color = Color.gray;
    }

    /// <summary>
    /// 在背景面板下创建关闭按钮，配置尺寸和外观，
    /// 并保存 Button 组件引用供后续绑定点击行为。
    /// </summary>
    private void CreateCloseButton()
    {
        // 创建按钮对象；此时对象带有 RectTransform，尚未添加 Image 和 Button
        GameObject buttonObject = new GameObject("CloseButton", typeof(RectTransform));
        buttonObject.transform.SetParent(_settingsPanel.transform, false);

        // 从按钮对象获取已经存在的 RectTransform，设置宽度和高度
        RectTransform closeButtonRect = buttonObject.GetComponent<RectTransform>();
        closeButtonRect.sizeDelta = new Vector2(120, 40);

        // 先添加 Image，才能通过它设置按钮的基础颜色
        Image buttonImage = buttonObject.AddComponent<Image>();
        buttonImage.color = Color.white;

        // 添加 Button 后，_closeButton 才保存了有效的按钮组件引用
        _closeButton = buttonObject.AddComponent<Button>();

        // 指定按钮悬停、按下等状态变化时使用的图形
        _closeButton.targetGraphic = buttonImage;
    }
}
