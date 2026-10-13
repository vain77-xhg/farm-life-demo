using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 事件管理器
/// 负责无参数事件的订阅、取消订阅和发布
/// </summary>
public class EventManager : MonoBehaviour
{
    //保存所有事件及其对应的回调方法
    private readonly Dictionary<string, Action> _eventTable = new Dictionary<string, Action>();
    
    /// <summary>
    /// 订阅指定事件。
    /// 如果事件已经存在，则追加新的回调方法；
    /// 如果事件不存在，则创建新的事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">事件触发时需要执行的方法。</param>
    public void Subscribe(string eventName, Action callback)
    {
        //如果事件已经存在，就追加新的回调方法
        if (_eventTable.ContainsKey(eventName))
        {
            _eventTable[eventName] += callback;
            return;
        }
        //如果事件不存在，就创建新的事件记录
        _eventTable.Add(eventName, callback);
    }
    
    /// <summary>
    /// 取消订阅指定事件。
    /// 如果事件存在，则移除对应的回调方法。
    /// 如果移除后已经没有任何回调方法，则删除该事件记录。
    /// </summary>
    /// <param name="eventName">事件名称。</param>
    /// <param name="callback">需要取消订阅的方法。</param>
    public void UnSubscribe(string eventName, Action callback)
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
}
