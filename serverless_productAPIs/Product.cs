using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverless_productAPIs
{
    public class Product
    {
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public int Product_Price { get; set; }
        public int Product_Quantity { get; set; }
        public string Category_Name { get; set; }
    }
}
