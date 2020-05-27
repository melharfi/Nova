using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Net.Messages
{
    internal static class CommandCaster
    {
        public static void Send(NetOutgoingMessage om)
        {
            // il faut utiliser cette classe pour envoyer les données, il faut remplacer common.SendMessage()
        }
    }
}
