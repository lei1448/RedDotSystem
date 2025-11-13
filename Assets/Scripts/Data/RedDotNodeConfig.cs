using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedDotNodeConfig")]
public class RedDotNodeConfig : ScriptableObject
{
    public List<RedDotNodeData> Nodes;
}
