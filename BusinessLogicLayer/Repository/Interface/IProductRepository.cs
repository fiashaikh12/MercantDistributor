using BusinessObjects.Entities;
using System.Collections.Generic;

namespace BusinessLogicLayer.Repository.Interface
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
