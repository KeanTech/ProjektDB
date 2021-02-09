using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SkpDbLib.Messages
{
    internal class Email : IMessage
    {
        private SmtpClient _smtpServer;
        private string _sender = "NoReply@zbc.dk";
        private MailMessage _email;

        public Email(string message, string reciever)
        {
            _smtpServer = new SmtpClient();
            _smtpServer.Host = "mail.efif.dk";
            _smtpServer.Port = 25;
            _smtpServer.EnableSsl = true;
            _smtpServer.UseDefaultCredentials = false;
            _smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            _email = new MailMessage();
            _email.From = new MailAddress(_sender);
            _email.To.Add(reciever);
            _email.Subject = "Projekt kommentar";
            _email.Body = message;
        }

        public void SendMessage()
        {
            _smtpServer.Send(_email);
            _email.Dispose();
        }
    }
}
