using HouseCommunity.Helpers;
using MailKit.Net.Smtp;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;
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
        public string SendMail(string subject, string content, string reciever, string senderName)
        {
            try
            {

                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                                "YOUR_API_KEY");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "YOUR_DOMAIN_NAME", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "Excited User <mailgun@YOUR_DOMAIN_NAME>");
                request.AddParameter("to", "bar@example.com");
                request.AddParameter("to", "YOU@YOUR_DOMAIN_NAME");
                request.AddParameter("subject", "Hello");
                request.AddParameter("text", "Testing some Mailgun awesomness!");
                request.Method = Method.POST;
                client.Execute(request);


                //EmailMessage message = new EmailMessage();
                //message.Sender = new MailboxAddress(senderName, _notificationMetadata.Sender);
                ////Test mail
                //message.Reciever = new MailboxAddress("Self", _notificationMetadata.Sender);
                //message.Subject = subject;
                //message.Content = content;
                //var mimeMessage = CreateMimeMessageFromEmailMessage(message);
                //using (SmtpClient smtpClient = new SmtpClient())
                //{
                //    smtpClient.Connect(_notificationMetadata.SmtpServer,
                //    _notificationMetadata.Port, true);
                //    smtpClient.Authenticate(_notificationMetadata.UserName,
                //    _notificationMetadata.Password);
                //    smtpClient.Send(mimeMessage);
                //    smtpClient.Disconnect(true);
                //}
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
        string SendMail(string subject, string content, string reciever, string nameSender);

    }
}
