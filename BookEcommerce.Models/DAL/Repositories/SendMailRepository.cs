using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class SendMailRepository : ISendMailRepository
    {
        private readonly MailSettings mailSettings;
        private readonly MimeMessage mimeMessage;
        private readonly SmtpClient smtpClient;
        public SendMailRepository(
            MailSettings mailSettings,
            MimeMessage mimeMessage, SmtpClient smtpClient)
        {
            this.mailSettings = mailSettings;
            this.mimeMessage = mimeMessage;
            this.smtpClient = smtpClient;
        }
        public async Task SendMailAsync(MailSendingViewModel SendMailDTO)
        {
            this.mimeMessage.Sender = new MailboxAddress(
                this.mailSettings.DisplayName,
                this.mailSettings.Mail
            );
            this.mimeMessage.From.Add(
                new MailboxAddress(
                    this.mailSettings.DisplayName,
                    this.mailSettings.Mail
                )
            );
            this.mimeMessage.To.Add(MailboxAddress.Parse(SendMailDTO.Email));
            this.mimeMessage.Subject = SendMailDTO.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = SendMailDTO.HtmlMessage;
            this.mimeMessage.Body = builder.ToMessageBody();

            try
            {
                smtpClient.Connect(
                    this.mailSettings.Host,
                    this.mailSettings.Port,
                    MailKit.Security.SecureSocketOptions.Auto
                );
                smtpClient.Authenticate(
                    this.mailSettings.Mail,
                    this.mailSettings.Password
                );
                await smtpClient.SendAsync(this.mimeMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("fail to send mail");
            }
        }
    }
}


