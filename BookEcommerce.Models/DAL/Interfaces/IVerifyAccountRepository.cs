using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface IVerifyAccountRepository
    {
        public Task<string> GenerateConfirmationLink(ApplicationUser User);
        public Task SendVerificationMail(MailSendingViewModel SendMailDTO);
        public Task<IdentityResult> ConfirmEmail(ApplicationUser User, string Token);
        public Task<string> CreateToken(ApplicationUser User);
        public Task<ApplicationUser> GetUser(ApplicationUser ApplicationUser);
    }
}
