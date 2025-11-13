using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedDotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nodeNameText;
    [SerializeField] private TextMeshProUGUI m_nodeCountText;
    [SerializeField] private Button m_selfBtn;
    [SerializeField] private GameObject m_redDot;
    public void Init(string name)
    {
        m_selfBtn.onClick.AddListener(
            () => {
                RedDotEvents.RaiseViewClicked(RedDotManager.Instance.GetNode(m_nodeNameText.text));
            }
        );
        m_nodeNameText.text = name;
        m_redDot.SetActive(false);
    }
    public void UpdateRedDot(bool isRed)
    {
        m_redDot.SetActive(isRed);
    }

    public void UpdateCounter(int count)
    {
        m_nodeCountText.text = count.ToString();
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
}
