using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedDotPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject m_redDotPanel;
    [SerializeField] private TextMeshProUGUI m_title;
    [SerializeField] private Button m_backBtn;
    [SerializeField] private GameObject m_childRedDotPanel;
    
    private bool m_isBackBusy = false;
    private Stack<RedDotNode> m_stack = new();
    private RedDotView[] m_childViews;

    void Awake()
    {
        m_childViews = m_childRedDotPanel.GetComponentsInChildren<RedDotView>(true);
        RedDotEvents.OnRedDotViewClicked += OnViewClicked;
        m_backBtn.onClick.AddListener(Back);
    }

    void Start()
    {
        m_redDotPanel.SetActive(false); 
    }

    private void OnViewClicked(RedDotNode node)
    {
        if (node.GetChildren().Count == 0)
        {
            return;
        }

        if (m_stack.Count > 0 && m_stack.Peek() == node)
        {
            return;
        }

        m_stack.Push(node);
        ShowPanel(node);
    }

    // public void Back()
    // {
    //     if (m_stack.Count > 0)
    //     {
    //         m_stack.Pop();
    //     }

    //     if (m_stack.Count == 0)
    //     {
    //         m_redDotPanel.SetActive(false);
    //         return;
    //     }

    //     ShowPanel(m_stack.Peek());
    // }

    public void Back()
    {
        if (m_isBackBusy) return; // 如果正在忙，阻止点击
        StartCoroutine(BackRoutine());
    }

    // 3. 真正的逻辑在协程里
    private IEnumerator BackRoutine()
    {
        m_isBackBusy = true; // 设置为“忙碌”

        if (m_stack.Count > 0)
        {
            m_stack.Pop();
        }

        if (m_stack.Count == 0)
        {
            m_redDotPanel.SetActive(false);
        }
        else
        {
            ShowPanel(m_stack.Peek());
        }
        
        // 4. 等待一帧结束
        yield return null; 

        m_isBackBusy = false; // 解除“忙碌”
    }
    private void ShowPanel(RedDotNode node)
    {
        m_title.text = node.Key;

        foreach (var view in m_childViews)
        {
            view.Hide();
        }

        List<RedDotNode> children = node.GetChildren();
        int num = 0;

        foreach (var child in children)
        {
            if (num >= m_childViews.Length)
            {
                break;
            }

            m_childViews[num].gameObject.SetActive(true);
            m_childViews[num].Init(child.Key);

            num++;
        }

        m_redDotPanel.SetActive(true);
    }

    void OnDestroy()
    {
        RedDotEvents.OnRedDotViewClicked -= OnViewClicked;
        m_backBtn.onClick.RemoveListener(Back);
    }
}