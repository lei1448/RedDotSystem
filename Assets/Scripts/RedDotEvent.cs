using System;
using System.Diagnostics;
public static class RedDotEvents
{
    public static event Action<RedDotNode> OnRedDotViewClicked;
    public static void RaiseViewClicked(RedDotNode node)
    {
        OnRedDotViewClicked?.Invoke(node);
    }

    public static event Action<bool,RedDotNode,int> OnRedDotStateChange;

    public static void RaiseRedDotStateChange(bool isRed,RedDotNode node,int count)
    {
        OnRedDotStateChange?.Invoke(isRed,node,count);
    }

    public static event Action<int> OnRedDotCounterChange;

    public static void RaiseRedDotCounterChange(int count)
    {
        OnRedDotCounterChange?.Invoke(count);
    }
}