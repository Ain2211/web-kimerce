using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Orders
{
    public class OrderItemModel : BaseModelInt
    {
        public int IdProduct { get; set; }

     

        public int IdOrder { get; set; }

        
        public int DisplayOrder { get; set; }

        public decimal Price { get; set; }

        public int NumberOfProduct { get; set; }

    }
}
