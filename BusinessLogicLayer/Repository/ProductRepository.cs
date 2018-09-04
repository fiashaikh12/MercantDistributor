using System;
using System.Collections.Generic;
using Repository;
using Entities;
using DataAccessLayer;
using System.Data;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(ProductDetails objProduct)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public List<ProductDetails> GetAllProductDetails()
        {
            List<ProductDetails> productDetails = new List<ProductDetails>();
            try {
                var dt = SqlHelper.GetTableFromSP("USP_GET_ALL_PRODUCTS");
                if(dt!=null && dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        productDetails.Add(new ProductDetails
                        {
                            Name = Convert.ToString(row["FULLNAME"]),
                            EmailAddress = Convert.ToString(row["EMAIL_ADDRESS"]),
                            Mobile = Convert.ToString(row["MOBILE_NUMBER"])
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return productDetails;
        }

        public List<ProductDetails> SalesReport()
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(ProductDetails objProduct)
        {
            throw new NotImplementedException();
        }
    }
}
