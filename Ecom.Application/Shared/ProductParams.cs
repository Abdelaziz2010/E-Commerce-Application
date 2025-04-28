using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Shared
{
    // convert prodcut parameters to complex type model 
    // to be used in the controller
    // to filter, sort and paginate the products
    public class ProductParams
    {
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        
        private int maxPageSize = 10;

        private int _pageSize = 3;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }
    }
}
