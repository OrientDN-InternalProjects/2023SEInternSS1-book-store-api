using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Services.Base;
using BookEcommerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookEcommerce.Services
{
    public class CartDetailService : BaseService, ICartDetailService
    {
        public CartDetailService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}