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
using Microsoft.Extensions.Logging;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class SendMailRepository : ISendMailRepository
    {
        private readonly MailSettings mailSettings;
        private readonly MimeMessage mimeMessage;
        private readonly SmtpClient smtpClient;
        private readonly ILogger<SendMailRepository> logger;
        public SendMailRepository(
            MailSettings mailSettings,
            MimeMessage mimeMessage, 
            SmtpClient smtpClient,
            ILogger<SendMailRepository> logger
        )
        {
            this.mailSettings = mailSettings;
            this.mimeMessage = mimeMessage;
            this.smtpClient = smtpClient;
            this.logger = logger;
        }

        public async Task SendMailAsync(MailSendingViewModel sendMailDTO)
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
            this.mimeMessage.To.Add(MailboxAddress.Parse(sendMailDTO.Email));
            this.mimeMessage.Subject = sendMailDTO.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = sendMailDTO.HtmlMessage;
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
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                throw new Exception("fail to send mail");
            }
        }
    }
}


