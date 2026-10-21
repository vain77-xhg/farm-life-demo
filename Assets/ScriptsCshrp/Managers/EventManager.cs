using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按事件名称保存回调，提供订阅、取消和发布方法。
/// </summary>
public class EventManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _eventTable：事件名称与无参数回调对应的字典。
    private readonly Dictionary<string, Action> _eventTable = new Dictionary<string, Action>();

    // _intEventTable：事件名称与 int 参数回调对应的字典。
    private readonly Dictionary<string, Action<int>> _intEventTable = new Dictionary<string, Action<int>>();

    // _stringEventTable：事件名称与 string 参数回调对应的字典。
    private readonly Dictionary<string, Action<string>> _stringEventTable = new Dictionary<string, Action<string>>();

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 添加无参数事件的回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="callback">事件发布时执行的回调方法。</param>
    public void Subscribe(string eventName, Action callback)
    {
        // 第一步：已有同名事件时追加回调。
        if (_eventTable.ContainsKey(eventName))
        {
            _eventTable[eventName] += callback;
            return;
        }

        // 第二步：没有同名事件时新建记录。
        _eventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 移除无参数事件的指定回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="callback">需要移除的回调方法。</param>
    public void Unsubscribe(string eventName, Action callback)
    {
        // 第一步：事件不存在时结束。
        if (!_eventTable.ContainsKey(eventName))
        {
            return;
        }

        // 第二步：移除指定回调。
        _eventTable[eventName] -= callback;

        // 第三步：没有剩余回调时删除事件记录。
        if (_eventTable[eventName] == null)
        {
            _eventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 发布无参数事件，调用已保存的回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    public void Publish(string eventName)
    {
        // 第一步：事件不存在时结束。
        if (!_eventTable.ContainsKey(eventName))
        {
            return;
        }

        // 第二步：调用事件保存的回调。
        _eventTable[eventName]?.Invoke();
    }

    /// <summary>
    /// 添加 int 参数事件的回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="callback">事件发布时执行的回调方法。</param>
    public void SubscribeInt(string eventName, Action<int> callback)
    {
        // 第一步：已有同名事件时追加回调。
        if (_intEventTable.ContainsKey(eventName))
        {
            _intEventTable[eventName] += callback;
            return;
        }

        // 第二步：没有同名事件时新建记录。
        _intEventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 移除 int 参数事件的指定回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="callback">需要移除的回调方法。</param>
    public void UnsubscribeInt(string eventName, Action<int> callback)
    {
        // 第一步：事件不存在时结束。
        if (!_intEventTable.ContainsKey(eventName))
        {
            return;
        }

        // 第二步：移除指定回调。
        _intEventTable[eventName] -= callback;

        // 第三步：没有剩余回调时删除事件记录。
        if (_intEventTable[eventName] == null)
        {
            _intEventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 发布 int 参数事件，调用已保存的回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="value">发送给订阅者的数据。</param>
    public void PublishInt(string eventName, int value)
    {
        // 第一步：事件不存在时结束。
        if (!_intEventTable.ContainsKey(eventName))
        {
            return;
        }

        // 第二步：调用事件保存的回调。
        _intEventTable[eventName]?.Invoke(value);
    }

    /// <summary>
    /// 添加 string 参数事件的回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="callback">事件发布时执行的回调方法。</param>
    public void SubscribeString(string eventName, Action<string> callback)
    {
        // 第一步：已有同名事件时追加回调。
        if (_stringEventTable.ContainsKey(eventName))
        {
            _stringEventTable[eventName] += callback;
            return;
        }

        // 第二步：没有同名事件时新建记录。
        _stringEventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 移除 string 参数事件的指定回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="callback">需要移除的回调方法。</param>
    public void UnsubscribeString(string eventName, Action<string> callback)
    {
        // 第一步：事件不存在时结束。
        if (!_stringEventTable.ContainsKey(eventName))
        {
            return;
        }

        // 第二步：移除指定回调。
        _stringEventTable[eventName] -= callback;

        // 第三步：没有剩余回调时删除事件记录。
        if (_stringEventTable[eventName] == null)
        {
            _stringEventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 发布 string 参数事件，调用已保存的回调。
    /// </summary>
    /// <param name="eventName">事件名称，用于查找对应回调。</param>
    /// <param name="value">发送给订阅者的数据。</param>
    public void PublishString(string eventName, string value)
    {
        // 第一步：事件不存在时结束。
        if (!_stringEventTable.ContainsKey(eventName))
        {
            return;
        }

        // 第二步：调用事件保存的回调。
        _stringEventTable[eventName]?.Invoke(value);
    }

    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================

}
