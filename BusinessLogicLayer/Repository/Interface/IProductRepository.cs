using Entities;
using System.Collections.Generic;

namespace Repository
{
    public interface IProductRepository
    {
        List<ProductDetails> SalesReport();
        void AddProduct(ProductDetails objProduct);
        void DeleteProduct(int productId);
        void UpdateProduct(ProductDetails objProduct);
        List<ProductDetails> GetAllProductDetails();
    }
}
