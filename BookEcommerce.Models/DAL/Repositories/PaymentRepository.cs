using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using Microsoft.Extensions.Logging;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = BookEcommerce.Models.Entities.Order;
using Payment = PayPal.Api.Payment;

namespace BookEcommerce.Models.DAL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PayPal.Api.Payment? Payment;
        private readonly ILogger<PaymentRepository> logger;
        public PaymentRepository(DbFactory dbFactory, ILogger<PaymentRepository> logger) : base(dbFactory)
        {
            this.logger = logger;
        }
        
        public ItemList GetItemList(Order order)
        {
            var itemList = new ItemList();
            foreach (var item in order.OrderDetails!)
            {
                itemList.items.Add(new Item
                {
                    name = item.ProductVariant!.ProductVariantName,
                    currency = "USD",
                    price = Convert.ToString(item.Price),
                    quantity = Convert.ToString(item.Quantity.ToString()),
                    sku = "sku"
                });
            };
            return itemList;
        }
        
        public Payer GetPayer()
        {
            var payer = new Payer()
            {
                //payer_info = new PayerInfo()
                //{
                //    email = email
                //},
                payment_method = "paypal"
            };
            return payer;
        }
        
        public Amount GetAmount(Order order)
        {
            var amount = new Amount
            {
                currency = "USD",
                total = order.TotalPrice.ToString()
            };
            return amount;
        }
        
        public List<Transaction> CreateTransactionList(Order order)
        {
            //var itemlist = GetItemList(order);
            var amount = GetAmount(order);
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction
            {
                description = "Paypal payment transaction",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                //item_list = itemlist
            });
            return transactionList;
        }
        
        public PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectURL, Order order)
        {
            var payer = GetPayer();
            var transactions = CreateTransactionList(order);
            var RedirUrls = new RedirectUrls()
            {
                cancel_url = redirectURL + "&Cancel=true",
                return_url = redirectURL
            };
            this.Payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactions,
                redirect_urls = RedirUrls
            };
            var paymentCreation = this.Payment.Create(apiContext);
            return paymentCreation;
        }

        public PayPal.Api.Payment ExecutePayment(APIContext ApiContext, string PayerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = PayerId,
            };
            this.Payment = new Payment
            {
                id = PaymentId
            };
            return this.Payment.Execute(ApiContext, paymentExecution);
        }
    }
}
