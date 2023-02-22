using BookEcommerce.Models.DTOs;
using BookEcommerce.Models.DTOs.Response.Base;
using BookEcommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services.Interfaces
{
    public interface IVerifyAccountService
    {
        public Task<string> GenerateConfirmationLink(ApplicationUser User);
        public Task SendVerificationMail(string Email);
        public Task<ResponseBase> ConfirmMail(string Email);
    }
}
