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

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
