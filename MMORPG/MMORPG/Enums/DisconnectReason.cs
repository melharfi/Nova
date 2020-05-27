namespace MMORPG.Enums
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
