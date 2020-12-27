using HouseCommunity.Helpers;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseCommunity.Services
{
    public class MailService : IMailService
    {
        private readonly NotificationMetadata _notificationMetadata;

        public MailService(NotificationMetadata notificationMetadata)
        {
            this._notificationMetadata = notificationMetadata;
        }
        public string SendMail(string subject, string content, string reciever)
        {
            try
            {

                EmailMessage message = new EmailMessage();
                message.Sender = new MailboxAddress("Self", _notificationMetadata.Sender);
                //Test mail
                message.Reciever = new MailboxAddress("Self", _notificationMetadata.Sender);
                message.Subject = subject;
                message.Content = content;
                var mimeMessage = CreateMimeMessageFromEmailMessage(message);
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_notificationMetadata.SmtpServer,
                    _notificationMetadata.Port, true);
                    smtpClient.Authenticate(_notificationMetadata.UserName,
                    _notificationMetadata.Password);
                    smtpClient.Send(mimeMessage);
                    smtpClient.Disconnect(true);
                }
            }
            catch(Exception ex)
            {
                return "Email not sent!";

            }
            return "Email sent successfully";
        }

        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            { Text = message.Content };
            return mimeMessage;
        }
    }

    public interface IMailService
    {
        string SendMail(string subject, string content, string reciever);

    }
}
