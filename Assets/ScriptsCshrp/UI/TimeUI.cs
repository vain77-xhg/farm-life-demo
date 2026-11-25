using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 在常驻时钟上分别显示季节、季节内天数和时间。
/// </summary>
public class TimeUI : UIBase
{
    // ==================== 字段 ====================

    // _timeText：预制体中用于显示游戏时间的文字组件。
    [SerializeField] private TMP_Text _timeText;

    // _dayText：预制体中用于显示本季第几天的文字组件。
    [SerializeField] private TMP_Text _dayText;

    // _SeasonText：预制体中用于显示当前季节名称的文字组件。
    [SerializeField] private TMP_Text _SeasonText;

    // _timeManager：提供真实游戏时间的管理器。
    private TimeManager _timeManager;

    // _dayFormat：保存查到的日期模板，例如“第{0}天”。
    private string _dayFormat;

    // _seasonNames：以季节枚举为键缓存四季的显示名称。
    private Dictionary<GameSeason, string> _seasonNames;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 时间管理器准备好后，逐帧刷新时钟界面。
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
    /// 保存时间管理器，缓存日期模板和四季名称，再刷新界面。
    /// </summary>
    /// <param name="timeManager">用于查询真实日期和时间的管理器。</param>
    /// <param name="textManager">用于查询日期模板和四季名称的管理器。</param>
    public void Initialize(TimeManager timeManager, TextManager textManager)
    {
        // 第一步：保存时间管理器引用。
        _timeManager = timeManager;

        // 第二步：按文本编号查询并缓存日期模板和四季名称。
        _dayFormat = textManager.GetText(TextIds.Calendar.DayFormat);

        _seasonNames = new Dictionary<GameSeason, string>
        {
            { GameSeason.Spring, textManager.GetText(TextIds.Calendar.Spring) },
            { GameSeason.Summer, textManager.GetText(TextIds.Calendar.Summer) },
            { GameSeason.Autumn, textManager.GetText(TextIds.Calendar.Autumn) },
            { GameSeason.Winter, textManager.GetText(TextIds.Calendar.Winter) }
        };

        // 第三步：刷新初始日期和时间。
        RefreshTime();
    }

    // ==================== 对外接口 ====================

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 查询当前日期和时间，分别更新三块文字。
    /// </summary>
    private void RefreshTime()
    {
        // 第一步：查询当前季节、季节内天数和真实时分。
        // day：当前季节内的第几天，从 1 开始。
        int day = _timeManager.GetDayOfSeason();

        // season：当前季节，用作字典的键。
        GameSeason season = _timeManager.GetSeason();

        // seasonName：查到的当前季节显示名称。
        string seasonName = _seasonNames[season];

        // hour：当前游戏小时。
        int hour = _timeManager.GetHour();

        // minute：当前真实游戏分钟。
        int minute = _timeManager.GetMinute();

        // 第二步：计算界面显示的分钟。
        // displayMinute：向下取整到十分钟的显示值。
        int displayMinute = minute / 10 * 10;

        // 第三步：分别更新季节、天数和时间控件。
        _SeasonText.text = seasonName;
        _dayText.text = string.Format(_dayFormat, day);
        _timeText.text = $"{hour:D2}:{displayMinute:D2}";
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
