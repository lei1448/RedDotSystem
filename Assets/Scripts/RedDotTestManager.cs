using UnityEngine;

public class RedDotTestManager : MonoBehaviour
{
    private RedDotNode m_mailUnreadNode;
    private RedDotNode m_mailAttachNode;
    private RedDotNode m_taskAvailableNode;
    private RedDotNode m_taskCompleteNode;
    private RedDotNode m_socialApplyNode;
    private RedDotNode m_socialChatNode;
    private RedDotNode m_bagNewItemNode;
    private RedDotNode m_bagCraftNode;

    void Start()
    {
        RedDotManager.Instance.InitializedEvent.AddListener(OnRedDotSystemReady);
    }

    private void OnRedDotSystemReady()
    {
        m_mailUnreadNode = RedDotManager.Instance.GetNode("邮件");
        m_mailAttachNode = RedDotManager.Instance.GetNode("附件");
        
        m_taskAvailableNode = RedDotManager.Instance.GetNode("可接受");
        m_taskCompleteNode = RedDotManager.Instance.GetNode("未完成");
        
        m_socialApplyNode = RedDotManager.Instance.GetNode("申请");
        m_socialChatNode = RedDotManager.Instance.GetNode("消息");
        
        m_bagNewItemNode = RedDotManager.Instance.GetNode("道具");
        m_bagCraftNode = RedDotManager.Instance.GetNode("合成");
    }

    // --- 邮件 ---
    public void Test_AddUnreadMail()
    {
        Debug.Log("触发：添加未读邮件");
        m_mailUnreadNode?.SetSelfCount(m_mailUnreadNode.GetCount() + 1);
    }

    public void Test_ClearUnreadMail()
    {
        Debug.Log("清除：减少一封未读邮件");
        m_mailUnreadNode?.SetSelfCount(m_mailUnreadNode.GetCount() - 1);
    }

    public void Test_AddAttachment()
    {
        Debug.Log("触发：添加附件");
        m_mailAttachNode?.SetSelfCount(1);
    }

    public void Test_ClearAttachment()
    {
        Debug.Log("清除：领取附件");
        m_mailAttachNode?.SetSelfCount(0);
    }

    // --- 任务 ---
    public void Test_AddTaskAvailable()
    {
        Debug.Log("触发：添加可接任务");
        m_taskAvailableNode?.SetSelfCount(m_taskAvailableNode.GetCount() + 1);
    }

    public void Test_ClearTaskAvailable()
    {
        Debug.Log("清除：减少一个可接任务");
        m_taskAvailableNode?.SetSelfCount(m_taskAvailableNode.GetCount() - 1);
    }

    public void Test_AddTaskComplete()
    {
        if (m_taskAvailableNode.IsRed)
        {
            Test_ClearTaskAvailable();
            Debug.Log("触发：添加可完成任务");
            m_taskCompleteNode?.SetSelfCount(m_taskCompleteNode.GetCount() + 1);
        } 
    }

    public void Test_ClearTaskComplete()
    {
        Debug.Log("清除：减少一个可完成任务");
        m_taskCompleteNode?.SetSelfCount(m_taskCompleteNode.GetCount() - 1);
    }

    // --- 社交 ---
    public void Test_AddFriendRequest()
    {
        Debug.Log("触发：添加好友申请");
        m_socialApplyNode?.SetSelfCount(m_socialApplyNode.GetCount() + 1);
    }

    public void Test_ClearFriendRequest()
    {
        Debug.Log("清除：减少一个好友申请");
        m_socialApplyNode?.SetSelfCount(m_socialApplyNode.GetCount() - 1);
    }
    
    public void Test_AddSocialMessage()
    {
        Debug.Log("触发：添加未读消息");
        m_socialChatNode?.SetSelfCount(m_socialChatNode.GetCount() + 1);
    }

    public void Test_ClearSocialMessage()
    {
        Debug.Log("清除：减少一条未读消息");
        m_socialChatNode?.SetSelfCount(m_socialChatNode.GetCount() - 1);
    }

    // --- 背包 ---
    public void Test_AddNewItem()
    {
        Debug.Log("触发：添加新道具");
        m_bagNewItemNode?.SetSelfCount(m_bagNewItemNode.GetCount() + 1);
        if(m_bagNewItemNode.GetCount() >= 3)
        {
            Test_AddCraftable();
        }
    }

    public void Test_ClearNewItem()
    {
        Debug.Log("清除：减少一个新道具");
        m_bagNewItemNode?.SetSelfCount(m_bagNewItemNode.GetCount() - 1);
    }
    
    public void Test_AddCraftable()
    {
        Debug.Log("触发：三个道具可合成");
        m_bagCraftNode?.SetSelfCount(1);
    }

    public void Test_ClearCraftable()
    {
        if(m_bagNewItemNode.GetCount() >= 3)
        {           
            Debug.Log("清除：合成三个道具");
            m_bagNewItemNode?.SetSelfCount(m_bagNewItemNode.GetCount() - 3);
            m_bagCraftNode?.SetSelfCount(m_bagNewItemNode.GetCount()/3);
        }
    }
}