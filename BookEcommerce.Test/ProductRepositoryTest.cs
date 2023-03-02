using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.Entities;
using Moq;

namespace BookEcommerce.Test
{
    [TestFixture]
    public class ProductRepositoryTest
    {
        private IEnumerable<Product>? products;
        Mock<IProductRepository>? productRepositoryMock;
        [SetUp]
        public void Setup()
        {
            products = new List<Product>
            {
                new Product
                {
                    ProductId = new Guid("45f7a4a8-d859-4433-c339-08db18899e0f"),
                    ProductName = "string",
                    ProductDecription = "string",
                    VendorId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    IsActive = true
                },
                new Product
                {
                    ProductId = new Guid("65a1b197-d51e-4fc5-a0d1-08db188c5303"),
                    ProductName = "productNameDemo",
                    ProductDecription = "string",
                    VendorId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    IsActive= true
                }
            };
            productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(pm => pm.GetProductById(It.IsAny<Guid>())).ReturnsAsync((Guid id) => products.Single(x=> x.ProductId.Equals(id)));
        }

        [Test]
        public async Task Get_product_when_productid_is_valid()
        {
            var productMockRepo = productRepositoryMock.Object;
            var res = await Task.FromResult(productMockRepo.GetProductById(new Guid("65a1b197-d51e-4fc5-a0d1-08db188c5303")));
            Assert.IsNotNull(res);
            Assert.That("productNameDemo",Is.EqualTo(res.Result.ProductName));
        }
    }
}