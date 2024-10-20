using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Models
{
    public class ProductRequest
    {
        public string ProdudctId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductPrice { get; set; } = null!;
        public string StockCount { get; set; } = null!;
        public bool InStock { get; set; }
        public bool IsUndo { get; set; }

    }
}
