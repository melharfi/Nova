using MELHARFI.Lidgren.Network;

namespace MMORPG.Net.Messages
{
    internal static class CommandCaster
    {
        public static void Send(NetOutgoingMessage om)
        {
            Network.netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            Network.netClient.FlushSendQueue();
        }
    }
}
