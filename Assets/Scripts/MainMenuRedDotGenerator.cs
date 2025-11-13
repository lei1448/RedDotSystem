using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRedDotGenerator : MonoBehaviour
{
    [SerializeField] private RedDotView m_redDotPre;
    [SerializeField] private Transform m_parent;

    // Start is called before the first frame update
    void Start()
    {

    }   
    

    public void Generate()
    {
        foreach(RedDotNode redDotNode in RedDotManager.Instance.GetNodesByFloor(2))
        {
            RedDotView redDotView = Instantiate(m_redDotPre, m_parent);
            redDotView.Init(redDotNode.Key);
        }
    }
}
