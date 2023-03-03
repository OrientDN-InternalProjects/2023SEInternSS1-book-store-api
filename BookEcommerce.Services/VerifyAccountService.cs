using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class VerifyAccountService : IVerifyAccountService
    {
        private readonly ISendMailRepository sendMailRepository;
        private readonly IVerifyAccountRepository verifyAccountRepository;
        private readonly IMapper mapper;
        private readonly ILogger logger;
        public VerifyAccountService(
            ISendMailRepository sendMailRepository,
            IMapper mapper,
            IVerifyAccountRepository verifyAccountRepository,
            ILogger logger
        )
        {
            this.sendMailRepository = sendMailRepository;
            this.mapper = mapper;
            this.verifyAccountRepository = verifyAccountRepository;
            this.logger = logger;
        }

        public async Task<ResponseBase> ConfirmMail(string email)
        {
            try
            {
                ApplicationUser user = new()
                {
                    Email = email
                };
                var getUser = await this.verifyAccountRepository.GetUser(user);
                string token = await this.verifyAccountRepository.CreateToken(getUser);
                var result = await this.verifyAccountRepository.ConfirmEmail(getUser, token);
                if (!result.Succeeded) return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "Invalid Mail"
                };
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Confirmed"
                };
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Invalid confirmation, please reconfirm!"
                };
            }
        }

        public async Task<string> GenerateConfirmationLink(ApplicationUser user)
        {
            string link = await this.verifyAccountRepository.GenerateConfirmationLink(user);
            return link;
        }

        public async Task<ResponseBase> SendVerificationMail(string email)
        {
            try
            {
                if (email == null)
                {
                    return new ResponseBase
                    {
                        IsSuccess = false,
                        Message = "invalid mail"
                    };
                }
                ApplicationUser User = new()
                {
                    Email = email
                };
                var link = await this.GenerateConfirmationLink(User);
                MailSendingViewModel SendMailDTO = new()
                {
                    Email = email,
                    Subject = "Verification",
                    HtmlMessage = link
                };
                await this.sendMailRepository.SendMailAsync(SendMailDTO);
                return new ResponseBase
                {
                    IsSuccess = true,
                    Message = "Mail sent"
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}. Detail {ex.StackTrace}");
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = "failed to send mail",
                };
                
            }
        }
    }
}
