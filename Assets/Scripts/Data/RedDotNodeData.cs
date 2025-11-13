using System;

public enum RedDotDisplayType
{
    DotAndCount, // 显示红点和数字
    DotOnly      // 只显示红点
}

[Serializable]
public class RedDotNodeData
{
    public string Key;
    public string ParentKey;
    
    public RedDotDisplayType DisplayType;
}
