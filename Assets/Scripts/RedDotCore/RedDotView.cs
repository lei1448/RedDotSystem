using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedDotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nodeNameText;
    [SerializeField] private TextMeshProUGUI m_nodeCountText;
    [SerializeField] private Button m_selfBtn;
    [SerializeField] private GameObject m_redDot;
    
    private string m_nodeKey;
    private RedDotNode m_node;
    private RedDotDisplayType m_displayType;

    public void Init(string key, bool isRed, int branchCount)
    {
        m_nodeKey = key;
        m_nodeNameText.text = m_nodeKey;
        
        m_node = RedDotManager.Instance.GetNode(m_nodeKey);
        if (m_node == null) return;
        m_displayType = m_node.DisplayType;

        m_selfBtn.onClick.RemoveAllListeners();
        RedDotEvents.OnRedDotStateChange -= OnRedDotStateChange;

        m_selfBtn.onClick.AddListener(OnClicked);
        RedDotEvents.OnRedDotStateChange += OnRedDotStateChange;
        UpdateView(isRed, branchCount);
    }
    
    private void OnClicked()
    {
        if (m_node != null)
        {
            RedDotEvents.RaiseViewClicked(m_node);
        }
    }

    private void OnRedDotStateChange(bool isRed, RedDotNode node, int branchCount)
    {
        if (node != null && node.Key == m_nodeKey)
        {
            UpdateView(isRed, branchCount);
        }
    }

    private void UpdateView(bool isRed, int branchCount)
    {
        m_redDot.SetActive(isRed);
        if (m_displayType == RedDotDisplayType.DotOnly)
        {
            m_nodeCountText.gameObject.SetActive(false);
        }
        else 
        {
            bool showCount = isRed && (branchCount > 0);
            m_nodeCountText.gameObject.SetActive(showCount);

            if (showCount)
            {
                m_nodeCountText.text = branchCount.ToString();
            }
        }
    }

    public void UpdateName(string name)
    {
        m_nodeNameText.text = name;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        RedDotEvents.OnRedDotStateChange -= OnRedDotStateChange;
        m_selfBtn.onClick.RemoveAllListeners();
    }
}