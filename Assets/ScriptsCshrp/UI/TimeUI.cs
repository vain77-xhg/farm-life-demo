using System;
using UnityEngine;
using TMPro;

/// <summary>
/// 查询游戏日期和时间，使用文本表模板更新常驻显示。
/// </summary>
public class TimeUI : UIBase
{
    // ==================== 字段 ====================

    // _timeText：预制体中用于显示游戏时间的文字组件。
    [SerializeField] private TMP_Text _timeText;

    // _dayText：预制体中用于显示游戏日序号的文字组件。
    [SerializeField] private TMP_Text _dayText;

    // _timeManager：提供真实游戏时间的管理器。
    private TimeManager _timeManager;

    // _dayFormatTextId：日期文字模板在文本表中的编号。
    [SerializeField] private int _dayFormatTextId = 400001;

    // _dayFormat：保存查到的日期模板，例如“第{0}天”。
    private string _dayFormat;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 时间管理器准备好后，逐帧刷新日期和时间。
    /// </summary>
    private void Update()
    {
        // 第一步：尚未初始化时，结束本帧处理。
        if (_timeManager == null)
        {
            return;
        }

        // 第二步：查询当前日期和时间，更新文字。
        RefreshTime();
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存时间管理器、读取日期模板并刷新初始显示。
    /// </summary>
    /// <param name="timeManager">用于查询真实日期和时间的管理器。</param>
    /// <param name="textManager">用于查询日期文字模板的管理器。</param>
    public void Initialize(TimeManager timeManager, TextManager textManager)
    {
        // 第一步：保存时间管理器引用。
        _timeManager = timeManager;

        // 第二步：查询并缓存日期模板。
        _dayFormat = textManager.GetText(_dayFormatTextId);

        // 第三步：刷新初始日期和时间。
        RefreshTime();
    }

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 读取游戏日期和时间，更新对应文字。
    /// </summary>
    private void RefreshTime()
    {
        // 第一步：读取真实日期和时间。
        // day：从 1 开始的游戏日序号。
        int day = _timeManager.GetDayOfSeason();

        // hour：当前游戏小时。
        int hour = _timeManager.GetHour();

        // minute：当前真实游戏分钟。
        int minute = _timeManager.GetMinute();

        // 第二步：计算界面显示的分钟。
        // displayMinute：向下取整到十分钟的显示值。
        int displayMinute = minute / 10 * 10;

        // 第三步：分别更新日期和时间文字。
        _dayText.text = string.Format(_dayFormat, day);
        _timeText.text = $"{hour:D2}:{displayMinute:D2}";
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
