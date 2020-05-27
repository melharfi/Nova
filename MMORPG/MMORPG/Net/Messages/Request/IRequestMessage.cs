namespace MMORPG.Net.Messages.Request
{
    public interface IRequestMessage
    {
        bool _serialized { get; set; }
        string _buffer { get; set; }
        void Send();
        void Serialize();
    }
}