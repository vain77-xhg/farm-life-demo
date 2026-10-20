using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理器。
/// 按名称保存回调，提供无参数、int 参数和 string 参数事件的订阅、取消订阅与发布接口。
/// 三种参数类型分别保存在各自的事件表中。
/// </summary>
public class EventManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // 事件名称对应一组无参数回调。
    private readonly Dictionary<string, Action> _eventTable = new Dictionary<string, Action>();

    // 事件名称对应一组接收 int 参数的回调。
    private readonly Dictionary<string, Action<int>> _intEventTable = new Dictionary<string, Action<int>>();

    // 事件名称对应一组接收 string 参数的回调。
    private readonly Dictionary<string, Action<string>> _stringEventTable = new Dictionary<string, Action<string>>();

    // ==================== 无参数事件 ====================

    /// <summary>
    /// 订阅无参数事件；同名事件已经存在时，将回调追加到现有记录中。
    /// </summary>
    /// <param name="eventName">事件名称，通常使用 GameEvents 中的常量。</param>
    /// <param name="callback">发布事件时需要调用的方法。</param>
    public void Subscribe(string eventName, Action callback)
    {
        if (_eventTable.ContainsKey(eventName))
        {
            _eventTable[eventName] += callback;
            return;
        }

        _eventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 移除无参数事件中的指定回调；最后一个回调被移除后，删除该事件记录。
    /// </summary>
    /// <param name="eventName">需要取消订阅的事件名称。</param>
    /// <param name="callback">订阅时传入的回调方法。</param>
    public void Unsubscribe(string eventName, Action callback)
    {
        if (!_eventTable.ContainsKey(eventName))
        {
            return;
        }

        _eventTable[eventName] -= callback;

        if (_eventTable[eventName] == null)
        {
            _eventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 发布无参数事件，调用当前记录的回调；没有事件记录时直接返回。
    /// </summary>
    /// <param name="eventName">需要发布的事件名称。</param>
    public void Publish(string eventName)
    {
        if (!_eventTable.ContainsKey(eventName))
        {
            return;
        }

        _eventTable[eventName]?.Invoke();
    }

    // ==================== int 参数事件 ====================

    /// <summary>
    /// 订阅携带 int 参数的事件；同名事件已经存在时追加回调。
    /// </summary>
    /// <param name="eventName">事件名称，通常使用 GameEvents 中的常量。</param>
    /// <param name="callback">接收一个 int 参数的回调方法。</param>
    public void SubscribeInt(string eventName, Action<int> callback)
    {
        if (_intEventTable.ContainsKey(eventName))
        {
            _intEventTable[eventName] += callback;
            return;
        }

        _intEventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 移除 int 参数事件中的指定回调；没有剩余回调时删除事件记录。
    /// </summary>
    /// <param name="eventName">需要取消订阅的事件名称。</param>
    /// <param name="callback">订阅时传入的 int 参数回调。</param>
    public void UnsubscribeInt(string eventName, Action<int> callback)
    {
        if (!_intEventTable.ContainsKey(eventName))
        {
            return;
        }

        _intEventTable[eventName] -= callback;

        if (_intEventTable[eventName] == null)
        {
            _intEventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 发布携带 int 数据的事件，将该数据传给当前记录的回调。
    /// </summary>
    /// <param name="eventName">需要发布的事件名称。</param>
    /// <param name="value">传给订阅者的整数数据。</param>
    public void PublishInt(string eventName, int value)
    {
        if (!_intEventTable.ContainsKey(eventName))
        {
            return;
        }

        _intEventTable[eventName]?.Invoke(value);
    }

    // ==================== string 参数事件 ====================

    /// <summary>
    /// 订阅携带 string 参数的事件；同名事件已经存在时追加回调。
    /// </summary>
    /// <param name="eventName">事件名称，通常使用 GameEvents 中的常量。</param>
    /// <param name="callback">接收一个 string 参数的回调方法。</param>
    public void SubscribeString(string eventName, Action<string> callback)
    {
        if (_stringEventTable.ContainsKey(eventName))
        {
            _stringEventTable[eventName] += callback;
            return;
        }

        _stringEventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 移除 string 参数事件中的指定回调；没有剩余回调时删除事件记录。
    /// </summary>
    /// <param name="eventName">需要取消订阅的事件名称。</param>
    /// <param name="callback">订阅时传入的 string 参数回调。</param>
    public void UnsubscribeString(string eventName, Action<string> callback)
    {
        if (!_stringEventTable.ContainsKey(eventName))
        {
            return;
        }

        _stringEventTable[eventName] -= callback;

        if (_stringEventTable[eventName] == null)
        {
            _stringEventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 发布携带 string 数据的事件，将该数据传给当前记录的回调。
    /// </summary>
    /// <param name="eventName">需要发布的事件名称。</param>
    /// <param name="value">传给订阅者的字符串数据。</param>
    public void PublishString(string eventName, string value)
    {
        if (!_stringEventTable.ContainsKey(eventName))
        {
            return;
        }

        _stringEventTable[eventName]?.Invoke(value);
    }
}
