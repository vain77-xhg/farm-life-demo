using System;
using UnityEngine;

/// <summary>
/// 保存、推进并查询游戏日期和时间。
/// </summary>
public class TimeManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _currentGameMinutes：从第 1 天 00:00 起累计的分钟数，初始为第 1 天 06:00，跨日不清零。
    private int _currentGameMinutes = 6 * 60;

    // _secondsPerGameMinute：推进一个游戏分钟所需的现实秒数。
    [SerializeField] private float _secondsPerGameMinute = 0.7f;

    // _elapsedRealSeconds：已累计、尚未用于推进的现实秒数。
    private float _elapsedRealSeconds = 0f;

    // _gameStateManager：用于查询时钟推进权限的状态管理器。
    private GameStateManager _gameStateManager;

    // _daysPerSeason：每个季节包含的游戏天数。
    private int _daysPerSeason = 28;

    // _seasonsPerYear：每个游戏年包含的季节数量。
    private int _seasonsPerYear = 4;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 累计现实时间并推进游戏分钟。
    /// </summary>
    private void Update()
    {
        // 第一步：不允许时间推进时，结束本帧计时。
        if (!_gameStateManager.CanTimeAdvance())
        {
            return;
        }

        // 第二步：推进所需秒数无效时，结束本帧处理。
        if (_secondsPerGameMinute <= 0f)
        {
            return;
        }

        // 第三步：累计本帧经过的现实秒数。
        _elapsedRealSeconds += Time.unscaledDeltaTime;

        // 第四步：逐次消耗足额秒数，推进分钟并保留余量。
        while (_elapsedRealSeconds >= _secondsPerGameMinute)
        {
            _elapsedRealSeconds -= _secondsPerGameMinute;
            _currentGameMinutes++;

            // 临时输出每个游戏分钟，供 Console 验证推进。
            Debug.Log("游戏时间：" + GetHour() + ":" + GetMinute());
        }
    }

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存游戏状态管理器引用。
    /// </summary>
    /// <param name="gameStateManager">用于查询时钟推进权限的状态管理器。</param>
    public void Initialize(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 查询当前时钟小时。
    /// </summary>
    /// <returns>0～23 的小时数。</returns>
    public int GetHour()
    {
        return _currentGameMinutes / 60 % 24;
    }

    /// <summary>
    /// 查询当前时钟分钟。
    /// </summary>
    /// <returns>0～59 的分钟数。</returns>
    public int GetMinute()
    {
        return _currentGameMinutes % 60;
    }

    /// <summary>
    /// 查询当前是第几个游戏日。
    /// </summary>
    /// <returns>从 1 开始的游戏日序号。</returns>
    public int GetDay()
    {
        return _currentGameMinutes / (24 * 60) + 1;
    }

    /// <summary>
    /// 查询当前是季节内的第几天。
    /// </summary>
    /// <returns>从 1 开始的季节内日期。</returns>
    public int GetDayOfSeason()
    {
        return (GetDay() - 1) % _daysPerSeason + 1;
    }

    /// <summary>
    /// 查询当前季节。
    /// </summary>
    /// <returns>当前季节对应的枚举值。</returns>
    public GameSeason GetSeason()
    {
        // seasonIndex：当前季节序号，0～3 分别对应春夏秋冬。
        int seasonIndex = (GetDay() - 1) / _daysPerSeason % _seasonsPerYear;

        return (GameSeason)seasonIndex;
    }

    /// <summary>
    /// 查询当前是第几个游戏年。
    /// </summary>
    /// <returns>从 1 开始的游戏年份。</returns>
    public int GetYear()
    {
        // daysPerYear：一个游戏年包含的天数。
        int daysPerYear = _daysPerSeason * _seasonsPerYear;
        return (GetDay() - 1) / daysPerYear + 1;
    }

    /// <summary>
    /// 将游戏时钟直接推进到起床时刻，并重置计时余量。
    /// </summary>
    public void Sleep()
    {
        // wakeUpMinutes：按当前入睡时刻算出的起床累计分钟数。
        int wakeUpMinutes = CalculateWakeUpMinutes();

        _currentGameMinutes = wakeUpMinutes;

        _elapsedRealSeconds = 0f;
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    /// <summary>
    /// 计算当前时刻之后的下一次早上 06:00。
    /// </summary>
    /// <returns>下一次 06:00 对应的累计游戏分钟数。</returns>
    private int GetNextMorningMinutes()
    {
        // todayStart：今天 00:00 对应的累计游戏分钟数。
        int todayStart = _currentGameMinutes - _currentGameMinutes % (24 * 60);

        // nextMorning：先取今天 06:00，必要时移到明天。
        int nextMorning = todayStart + 6 * 60;

        if (nextMorning <= _currentGameMinutes)
        {
            nextMorning += 24 * 60;
        }

        return nextMorning;
    }

    /// <summary>
    /// 按当前入睡时刻计算起床时间。
    /// </summary>
    /// <returns>起床时刻对应的累计游戏分钟数。</returns>
    private int CalculateWakeUpMinutes()
    {
        // normalWakeUp：早睡时的目标，也就是下一次 06:00。
        int normalWakeUp = GetNextMorningMinutes();

        // sleepDeadline：这次 06:00 之前的凌晨 01:00，仍可按早睡规则起床。
        int sleepDeadline = normalWakeUp - 5 * 60;

        if (_currentGameMinutes <= sleepDeadline)
        {
            return normalWakeUp;
        }

        // 超过凌晨 01:00 时，睡满 7 个游戏小时。
        return _currentGameMinutes + 7 * 60;
    }

    // ==================== 调试显示 ====================

    /// <summary>
    /// 从第 1 天 23:00 测试睡眠后的时间跳转。
    /// </summary>
    [ContextMenu("测试睡眠：23点入睡")]
    private void DebugSleepAtNight()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        // 固定入睡时刻，方便在编辑器中重复验证。
        _currentGameMinutes = 23 * 60;
        Sleep();
    }
}
