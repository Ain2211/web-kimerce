using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Email
{
    public class EmailProviderItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LimitNumber { get; set; } 
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
