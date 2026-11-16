using System;
using UnityEngine;

/// <summary>
/// 保存、推进并查询游戏时间。
/// </summary>
public class TimeManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _currentGameMinutes：从 00:00 起计算的游戏分钟数，初始为 06:00。
    private int _currentGameMinutes = 6 * 60;

    // _secondsPerGameMinute：推进一个游戏分钟所需的现实秒数。
    [SerializeField] private float _secondsPerGameMinute = 0.7f;

    // _elapsedRealSeconds：已累计、尚未用于推进的现实秒数。
    private float _elapsedRealSeconds = 0f;

    // ==================== Unity 生命周期 ====================

    /// <summary>
    /// 累计现实时间并推进游戏分钟。
    /// </summary>
    private void Update()
    {
        // 第一步：推进所需秒数无效时，结束本帧处理。
        if (_secondsPerGameMinute <= 0f)
        {
            return;
        }

        // 第二步：累计本帧经过的现实秒数。
        _elapsedRealSeconds += Time.unscaledDeltaTime;

        // 第三步：逐次消耗足额秒数，推进分钟并保留余量。
        while (_elapsedRealSeconds >= _secondsPerGameMinute)
        {
            _elapsedRealSeconds -= _secondsPerGameMinute;
            _currentGameMinutes++;

            // 临时输出每个游戏分钟，供 Console 验证推进。
            Debug.Log("游戏时间：" + GetHour() + ":" + GetMinute());
        }
    }

    // ==================== 初始化 ====================

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

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
}
