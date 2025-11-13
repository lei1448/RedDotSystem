using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedDotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nodeNameText;
    [SerializeField] private TextMeshProUGUI m_nodeCountText;
    [SerializeField] private GameObject m_redDot;

    public void Init(string name)
    {
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
}
