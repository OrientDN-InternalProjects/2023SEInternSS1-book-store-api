using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class VerifyAccountRepository : IVerifyAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISendMailRepository sendMailRepository;
        public VerifyAccountRepository(UserManager<ApplicationUser> userManager, ISendMailRepository sendMailRepository)
        {
            this.userManager = userManager;
            this.sendMailRepository = sendMailRepository;
        }

        public async Task<ApplicationUser> GetUser(ApplicationUser user)
        {
            var indicatedUser = await this.userManager.FindByEmailAsync(user.Email);
            return indicatedUser;
        }

        public async Task<IdentityResult> ConfirmEmail(ApplicationUser user, string token)
        {
            var indicatedUser = await this.GetUser(user);
            var result = await this.userManager.ConfirmEmailAsync(indicatedUser, token);
            return result;
        }

        public async Task<string> CreateToken(ApplicationUser user)
        {
            var token = await this.userManager!.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<string> GenerateConfirmationLink(ApplicationUser user)
        {
            var token = await this.userManager!.GenerateEmailConfirmationTokenAsync(user);
            var encodeToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"https://localhost:7018/api/VerifyAccount/submit?token={encodeToken}&email={user.Email}";
            return confirmationLink;
        }

        public async Task SendVerificationMail(MailSendingViewModel sendMailDTO)
        {
            await this.sendMailRepository.SendMailAsync(sendMailDTO);
        }
    }
}
