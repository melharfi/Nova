using MELHARFI.Lidgren.Network;

namespace MMORPG.Cryptography.Algo
{
    static class NetworkEncryption
    {
        public static NetOutgoingMessage Encrypt(string buffer)
        {
            NetOutgoingMessage om = Network.netClient.CreateMessage(buffer);
            om.Encrypt(Network.algo);
            return om;
        }
    }
}
