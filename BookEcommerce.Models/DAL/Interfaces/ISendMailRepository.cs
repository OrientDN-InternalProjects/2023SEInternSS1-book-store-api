using BookEcommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Models.DAL.Interfaces
{
    public interface ISendMailRepository
    {
        public Task SendMailAsync(MailSendingViewModel SendMailDTO);
    }
}
