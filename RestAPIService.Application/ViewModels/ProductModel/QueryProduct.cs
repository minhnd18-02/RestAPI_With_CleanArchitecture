using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels.ProductModel
{
    public class QueryProduct
    {
        public string? CategoryName { get; set; }
        public string? Name { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
    }
}
