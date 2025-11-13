using System;
using System.Collections.Generic;
using UnityEngine;

public class RedDotNode
{ 
    public string Key { get; private set; }
    public bool IsRed { get; private set; }
    public RedDotNode Parent { get; private set; }
    private Dictionary<string, RedDotNode> m_children = new();
    private int m_selfCounter;
    public event Action<bool> OnStateChange;
    public RedDotNode(string key)
    {
        Key = key;
    }

    public void AddChild(RedDotNode childNode)
    {
        if (childNode == null || m_children.ContainsKey(childNode.Key))
        {
            return;
        }

        childNode.Parent = this;
        m_children.Add(childNode.Key, childNode);
    }

    public List<RedDotNode> GetChildren()
    {
        return new List<RedDotNode>(m_children.Values);
    }

    /// <summary>
    /// 外部调用，模拟获取新消息。同时更新状态，通知UI
    /// </summary>
    /// <param name="count">消息数量</param>
    public void SetSelfCount(int count)
    {
        m_selfCounter = count;
        RefreshState();
    }
    
    /// <summary>
    /// 更新状态，通知UI
    /// </summary>
    private void RefreshState()
    {
        bool newState = false;
        if (m_selfCounter > 0)
        {
            newState = true;
        }
        foreach (var child in m_children.Values)
        {
            if (child.IsRed)
            {
                newState = true;
                break;
            }
        }
        if (newState == IsRed)
        {
            return;
        }

        IsRed = newState;
        OnStateChange?.Invoke(IsRed);  
        Parent?.RefreshState();
    }
}
