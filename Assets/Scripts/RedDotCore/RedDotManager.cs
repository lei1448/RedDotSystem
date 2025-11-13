using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RedDotManager : MonoBehaviour
{
    [SerializeField] private RedDotNodeConfig m_rootConfig;
    public UnityEvent InitializedEvent;
    private RedDotNode m_root;
    private Dictionary<string, RedDotNode> m_nodes = new();

    public static RedDotManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (m_rootConfig == null || m_rootConfig.Nodes == null)
        {
            Debug.LogError("红点配置没有设置");
            return;
        }

        foreach (var entry in m_rootConfig.Nodes)
        {
            if (string.IsNullOrEmpty(entry.Key)) continue;

            if (m_nodes.ContainsKey(entry.Key))
            {
                Debug.LogWarning($"重复的 Key：{entry.Key}，已跳过");
                continue;
            }

            RedDotNode node = new(entry.Key);
            m_nodes.Add(entry.Key, node);

            node.SetDisplayType(entry.DisplayType);

            if (string.IsNullOrEmpty(entry.ParentKey))
            {
                m_root = node;
            }
        }

        foreach (var entry in m_rootConfig.Nodes)
        {
            if (string.IsNullOrEmpty(entry.Key) || string.IsNullOrEmpty(entry.ParentKey))
            {
                continue;
            }
            if (m_nodes.TryGetValue(entry.Key, out RedDotNode childNode))
            {
                if (m_nodes.TryGetValue(entry.ParentKey, out RedDotNode parentNode))
                {
                    parentNode.AddChild(childNode);
                }
                else
                {
                    Debug.LogWarning($"找不到 Key 为 {entry.Key} 的父节点: {entry.ParentKey}");
                }
            }
        }

        if (m_root == null) 
        {
            Debug.LogError("红点树配置中未找到根节点");
        }

        InitializedEvent?.Invoke();
    }

    public RedDotNode GetNode(string key)
    {
        if (m_nodes.ContainsKey(key))
            return m_nodes[key];
        else
        {
            Debug.LogWarning($"不存在节点:{key}");
            return null;
        }
    }

    public List<RedDotNode> GetNodesByFloor(int floor)
    {
        if (m_root == null) return null;
        List<RedDotNode> nodes = new();
        Queue<RedDotNode> q = new();
        int floorNum = 1;
        q.Enqueue(m_root);
        int size;
        while(q.Count != 0)
        {
            Debug.Log($"正在遍历第{floorNum}层");
            size = q.Count;
            while (size > 0)
            {
                RedDotNode newNode = q.Dequeue();
                if (floorNum == floor)
                {
                    nodes.Add(newNode);
                    size--;
                    continue;
                }
                foreach (var childNode in newNode.GetChildren())
                {
                    q.Enqueue(childNode);
                }
                size--;
            }
            floorNum++;
        }
        return nodes;
    }

}
