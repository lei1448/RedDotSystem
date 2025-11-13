using System;
using System.Diagnostics;
public static class RedDotEvents
{
    public static event Action<RedDotNode> OnRedDotViewClicked;
    public static void RaiseViewClicked(RedDotNode node)
    {
        OnRedDotViewClicked?.Invoke(node);
    }
}