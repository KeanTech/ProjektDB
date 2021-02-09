using SkpDbLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkpDbLib.Managers
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
                    _media.SendMessage();
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
