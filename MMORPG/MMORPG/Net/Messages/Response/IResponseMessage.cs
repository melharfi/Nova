using System;

namespace MMORPG.Net.Messages.Response
{
    public interface IResponseMessage
    {
        void Fetch(string[] commandStrings);
    }
}