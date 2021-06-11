using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Dto.Models;
using Kimerce.Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Orders
{
    public class OrderItem : BaseModelInt
    {
        public Status status { get; set; }

        


        public int value { get; set; } 

        public string transactionsid { get; set; } 

       

        public string shortDescription { get; set; } 

        public string customerid { get; set; } 

        public string userid { get; set; } 
        

        public DateTimeOffset? CreatedTime { get; set; }

        public string CreatedTimeDisplay => CreatedTime.HasValue ? CreatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";

        
        public DateTimeOffset? UpdatedTime { get; set; }

        public string UpdatedTimeDisplay => UpdatedTime.HasValue ? UpdatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";
    }
}
