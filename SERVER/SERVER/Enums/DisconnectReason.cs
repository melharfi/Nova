using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Enums
{
    public class DisconnectReason
    {
        public enum disconnectReason
        {
            HOST_UNREACHABLE,
            USER_BANNED,
            MAINTENANCE,
            RESTARTING,
            SHUTDOWN,
            WRONG_CREDENTIALS,
            IP_BANNED,
            INVALID_TYPES,
            SELF_DISCONNECT,
            ANOTHER_USER_OVERRIDE_CONNEXION,
            TIME_OUT,
            OTHER
        }
    }
}
