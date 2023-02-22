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

        public async Task<ApplicationUser> GetUser(ApplicationUser User)
        {
            var user = await this.userManager.FindByEmailAsync(User.Email);
            return user;
        }

        public async Task<IdentityResult> ConfirmEmail(ApplicationUser User, string Token)
        {
            var user = await this.GetUser(User);
            var result = await this.userManager.ConfirmEmailAsync(user, Token);
            return result;
        }

        public async Task<string> CreateToken(ApplicationUser User)
        {
            string Token = await this.userManager!.GenerateEmailConfirmationTokenAsync(User);
            return Token;
        }

        public async Task<string> GenerateConfirmationLink(ApplicationUser User)
        {
            string Token = await this.userManager!.GenerateEmailConfirmationTokenAsync(User);
            string EncodeToken = HttpUtility.UrlEncode(Token);
            string ConfirmationLink = $"https://localhost:7018/api/VerifyAccount?token={EncodeToken}&email={User.Email}";
            return ConfirmationLink;
        }

        public async Task SendVerificationMail(SendMailDTO SendMailDTO)
        {
            await this.sendMailRepository.SendMailAsync(SendMailDTO);
        }


    }
}
