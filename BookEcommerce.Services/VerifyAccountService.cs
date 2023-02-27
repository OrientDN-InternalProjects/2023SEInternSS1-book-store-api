using AutoMapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using BookEcommerce.Services.Interfaces;
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
        public VerifyAccountService(
            ISendMailRepository sendMailRepository,
            IMapper mapper,
            IVerifyAccountRepository verifyAccountRepository
        )
        {
            this.sendMailRepository = sendMailRepository;
            this.mapper = mapper;
            this.verifyAccountRepository = verifyAccountRepository;
        }

        public async Task<ResponseBase> ConfirmMail(string Email)
        {
            ApplicationUser user = new()
            {
                Email = Email
            };
            var getUser = await this.verifyAccountRepository.GetUser(user);
            string Token = await this.verifyAccountRepository.CreateToken(getUser);
            var result = await this.verifyAccountRepository.ConfirmEmail(getUser, Token);
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

        public async Task<string> GenerateConfirmationLink(ApplicationUser User)
        {
            string link = await this.verifyAccountRepository.GenerateConfirmationLink(User);
            return link;
        }

        public async Task<ResponseBase> SendVerificationMail(string Email)
        {
            try
            {
                if (Email == null)
                {
                    return new ResponseBase
                    {
                        IsSuccess = false,
                        Message = "invalid mail"
                    };
                }
                ApplicationUser User = new()
                {
                    Email = Email
                };
                var link = await this.GenerateConfirmationLink(User);
                MailSendingViewModel SendMailDTO = new()
                {
                    Email = Email,
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
            catch (Exception e)
            {
                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = e.Message,
                };
                
            }
        }
    }
}
