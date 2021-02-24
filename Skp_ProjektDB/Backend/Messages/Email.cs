using System.Net;
using System.Net.Mail;

namespace Skp_ProjektDB.Backend.Messages
{
    internal class Email : IMessage
    {
        public Email(string message, string reciever)
        {
            // need another mail server to send mails from

            SmtpClient _smtpServer = new SmtpClient();
            _smtpServer.Host = "mail.efif.dk";
            _smtpServer.Port = 25;
            _smtpServer.EnableSsl = true;
            _smtpServer.UseDefaultCredentials = false;
            _smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage _email = new MailMessage();
            _email.From = new MailAddress("NoReply@zbc.dk");
            _email.To.Add(reciever);
            _email.Subject = "Projekt kommentar";
            _email.Body = message;

            _smtpServer.Send(_email);
        }
    }
}
