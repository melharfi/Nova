using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    interface IResponseMessage
    {
        object[] CommandStrings { get; set; }
        NetConnection Nc { get; set; }
        bool _serialized { get; set; }
        string _buffer { get; set; }
        void Initialize(object[] commandStrings, NetConnection nc);
        void Send();
        void Serialize();
    }
}
