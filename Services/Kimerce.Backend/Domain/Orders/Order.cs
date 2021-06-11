using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Domain.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Orders
{
    public class Order : BaseEntityByInt
    {
        public Order()
        {
            value = 0;
            transactionsid = "";
            shortDescription = "";
            status = Status.Editing;
            customerid = "";
            userid = "";
        }
        public int value { get; set; } = 0;
         
        public string transactionsid { get; set; }


        [MaxLength(1024)]
        public string shortDescription { get; set; } = "";
        public Status status { get; set; }

        public string customerid { get; set; }

        public string userid { get; set; }


        public List<Order_Item> OrderItems { get; set; }
    }

    public enum Status
    {
        [Description("Đã hoàn thành")]
        Editing = 1,
        [Description("Chưa hoàn thành")]
        Pending = 0,
       
    }

}





// public ICollection<OrderAndProduct> o_a_pList { set; get; }
// public string name { get; set; }