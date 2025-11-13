using System;
using System.Collections.Generic;
using UnityEngine;

public class RedDotNode
{ 
    public string Key { get; private set; }
    public bool IsRed { get; private set; }
    public RedDotNode Parent { get; private set; }
    private Dictionary<string, RedDotNode> m_children = new();
    public int BranchCount { get; private set; }
    private int m_selfCounter;
    public RedDotDisplayType DisplayType { get; private set; }
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

    public int GetCount()
    {
        return m_selfCounter;
    }

    public void SetDisplayType(RedDotDisplayType type)
    {
        DisplayType = type;
    }
    /// <summary>
    /// 外部调用，模拟获取新消息。同时更新状态，通知UI
    /// </summary>
    /// <param name="count">消息数量</param>
    public void SetSelfCount(int count)
    {
        if (count < 0) count = 0;


        if (m_selfCounter == count) return;

        m_selfCounter = count;
        RefreshState();
    }
    
    /// <summary>
    /// 更新状态，通知UI
    /// </summary>
    private void RefreshState()
    {
        bool oldIsRed = IsRed;
        int oldBranchCount = BranchCount;

        IsRed = (m_selfCounter > 0); 
        BranchCount = m_selfCounter;

        foreach (var child in m_children.Values)
        {
            if (child.IsRed)
            {
                IsRed = true;
            }

            if (child.DisplayType != RedDotDisplayType.DotOnly)
            {
                BranchCount += child.BranchCount;
            }
        }

        bool stateChanged = (oldIsRed != IsRed) || (oldBranchCount != BranchCount);

        if (stateChanged)
        {
            RedDotEvents.RaiseRedDotStateChange(IsRed, this, BranchCount);
            
            Parent?.RefreshState();
        }
    }
}
