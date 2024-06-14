using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace serverless_productAPIs
{
    public static class DeleteProduct
    {
        [FunctionName("DeleteProduct")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route ="product/{id:int}")] HttpRequest req, int id, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                using (SqlConnection connection = new SqlConnection
               (Environment.GetEnvironmentVariable("SQLAZUREDBConnectionString")))
                {
                    string queryString = @"DELETE FROM [dbo].[ProductInformation] WHERE [Product_ID] = @Product_ID";
                    using (SqlCommand cmd = new SqlCommand(queryString))
                    {
                        cmd.Parameters.AddWithValue("@Product_ID", id);
                        cmd.Connection = connection;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return new OkObjectResult("Product record were deleted successfully ");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
