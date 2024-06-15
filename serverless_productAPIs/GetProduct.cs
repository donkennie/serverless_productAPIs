using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace serverless_productAPIs
{
    public static class GetProduct
    {
        [FunctionName("GetProduct")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product")] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                List<Product> productData = new List<Product>();
                using (SqlConnection connection = new SqlConnection
               (Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DBConnectionString", EnvironmentVariableTarget.Process)))
                {
                    string queryString = @"SELECT [Product_ID]
                                         ,[Product_Name]
                                        ,[Product_Description]
                                        ,[Product_Price]
                                        ,[Product_Quantity]
                                        ,[Category_Name]
                                         FROM [dbo].[ProductInformation]";
                    SqlCommand cmd = new SqlCommand(queryString, connection);
                    connection.Open();
                    using (SqlDataReader oReader = cmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            Product productInfo = new Product();
                            productInfo.Product_ID = (int)oReader["Product_ID"];
                            productInfo.Product_Name = oReader["Product_Name"].ToString();
                            productInfo.Product_Description = oReader
                           ["Product_Description"].ToString();
                            productInfo.Product_Price = (int)oReader
                           ["Product_Price"];
                            productInfo.Product_Quantity = (int)oReader
                           ["Product_Quantity"];
                            productInfo.Category_Name = oReader["Category_Name"].ToString();
                            productData.Add(productInfo);
                        }
                        connection.Close();
                    }
                }
                return new OkObjectResult(productData);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
