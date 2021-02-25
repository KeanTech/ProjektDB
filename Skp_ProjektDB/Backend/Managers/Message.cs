using Skp_ProjektDB.Backend.Messages;
using System;

namespace Skp_ProjektDB.Backend.Managers
{
    public class Message : IDisposable
    {
        private IMessage _media = null;
        public void SendMessage(Mediatype media, string message, string reciever)
        {
            switch (media)
            {
                case Mediatype.Email:
                    _media = new Email(message, reciever);
                    break;
                default:
                    break;
            }
        }
        public void Dispose()
        {
            _media = null;
        }
    }
}
