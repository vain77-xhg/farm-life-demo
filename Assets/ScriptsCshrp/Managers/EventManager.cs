using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件管理器。
/// 负责游戏事件的订阅、取消订阅和发布，
/// 当前支持无参数事件、携带 int和string 参数的事件。
/// </summary>
public class EventManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // 保存无参数事件
    private readonly Dictionary<string, Action> _eventTable = new Dictionary<string, Action>();
    
    // 保存携带 int 参数的事件
    private readonly Dictionary<string, Action<int>> _intEventTable = new Dictionary<string, Action<int>>();
    
    // 保存携带 string 参数的事件
    private readonly Dictionary<string, Action<string>> _stringEventTable = new Dictionary<string, Action<string>>();

    // ==================== 事件订阅 ====================

    /// <summary>
    /// 订阅无参数事件。
    /// 如果事件已经存在，则追加新的回调方法；
    /// 如果事件不存在，则创建新的事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">事件触发时需要执行的方法。</param>
    public void Subscribe(string eventName, Action callback)
    {
        // 如果事件已经存在，就追加新的回调方法
        if (_eventTable.ContainsKey(eventName))
        {
            _eventTable[eventName] += callback;
            return;
        }
        // 如果事件不存在，就创建新的事件记录
        _eventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 订阅一个携带 int 参数的事件。
    /// 如果事件已经存在，则追加新的回调方法；
    /// 如果事件不存在，则创建新的事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">事件触发时需要执行的回调方法。</param>
    public void SubscribeInt(string eventName, Action<int> callback)
    {
        // 如果事件已经存在，就追加新的回调方法
        if (_intEventTable.ContainsKey(eventName))
        {
            _intEventTable[eventName] += callback;
            return;
        }
        // 如果事件不存在，就创建新的事件记录
        _intEventTable.Add(eventName, callback);
    }

    /// <summary>
    /// 订阅一个携带 string 参数的事件。
    /// 如果事件已经存在，则追加新的回调方法；
    /// 如果事件不存在，则创建新的事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">事件触发时需要执行的回调方法。</param>
    public void SubscribeString(string eventName, Action<string> callback)
    {
        // 如果事件已经存在，就追加新的回调方法
        if (_stringEventTable.ContainsKey(eventName))
        {
            _stringEventTable[eventName] += callback;
            return;
        }

        // 如果事件不存在，就创建新的事件记录
        _stringEventTable.Add(eventName, callback);
    }
    // ==================== 取消订阅 ====================
    
    /// <summary>
    /// 取消订阅指定事件。
    /// 如果事件存在，则移除对应的回调方法。 
    /// 如果移除后已经没有任何回调方法，则删除该事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">需要取消订阅的方法。</param>
    public void Unsubscribe(string eventName, Action callback)
    {
        // 如果事件不存在，就直接结束
        if (!_eventTable.ContainsKey(eventName))
        {
            return;
        }

        // 从事件中移除指定的回调方法
        _eventTable[eventName] -= callback;

        // 如果这个事件已经没有任何回调方法，就删除这条事件记录
        if (_eventTable[eventName] == null)
        {
            _eventTable.Remove(eventName);
        }
    }

    /// <summary>
    /// 取消订阅一个携带 int 参数的事件。
    /// 如果事件存在，则移除对应的回调方法。
    /// 如果移除后已经没有任何回调方法，则删除该事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">需要取消订阅的回调方法。</param>
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
    /// 取消订阅一个携带 string 参数的事件。
    /// 如果事件存在，则移除对应的回调方法。
    /// 如果移除后已经没有任何回调方法，则删除该事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">需要取消订阅的回调方法。</param>
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
    // ==================== 事件发布 ====================

    /// <summary>
    /// 发布指定事件。
    /// 如果事件存在，则执行该事件下所有已订阅的回调方法。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    public void Publish(string eventName)
    {
        // 如果事件不存在，就直接结束
        if (!_eventTable.ContainsKey(eventName))
        {
            return;
        }

        // 执行这个事件下所有已订阅的方法
        _eventTable[eventName]?.Invoke();
    }

    /// <summary>
    /// 发布一个携带 int 参数的事件。
    /// 如果事件存在，则把指定的 int 数据传给所有订阅者。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="value">随事件一起传递的 int 数据。</param>
    public void PublishInt(string eventName, int value)
    {
        // 如果事件不存在，就直接结束
        if (!_intEventTable.ContainsKey(eventName))
        {
            return;
        }
        // 执行所有订阅方法，并把 value 传给它们
        _intEventTable[eventName]?.Invoke(value);
    }
    
    /// <summary>
    /// 发布一个携带 string 参数的事件。
    /// 如果事件存在，则把指定的 string 数据传给所有订阅者。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="value">随事件一起传递的 string 数据。</param>
    public void PublishString(string eventName, string value)
    {
        // 如果事件不存在，就直接结束
        if (!_stringEventTable.ContainsKey(eventName))
        {
            return;
        }
        // 执行所有订阅方法，并把 value 传给它们
        _stringEventTable[eventName]?.Invoke(value);
    }
}
