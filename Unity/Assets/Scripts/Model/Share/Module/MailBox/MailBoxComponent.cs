namespace ET
{
    public struct MailBoxInvoker
    {
        public Address FromAddress;
        public MessageObject MessageObject;
        public MailBoxComponent MailBoxComponent;
    }
    
    /// <summary>
    /// 挂上这个组件表示该Entity是一个Actor,接收的消息将会队列处理
    /// </summary>
    [ComponentOf]
    public class MailBoxComponent: Entity, IAwake<MailBoxType>, IDestroy
    {
        public long ParentInstanceId { get; set; }
        // Mailbox的类型
        public MailBoxType MailBoxType { get; set; }
    }
}