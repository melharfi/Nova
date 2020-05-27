using Lidgren.Network;

namespace SERVER.Net.Messages.Request
{
    internal interface IRequestMessage
    {
        object[] CommandStrings { get; set; }
        NetConnection Nc { get; set; }
        void Initialize(object[] commandStrings, NetConnection nc);
        bool Check();
        void Apply();
    }
}
