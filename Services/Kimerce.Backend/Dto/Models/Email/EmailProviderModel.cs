using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Email
{
    public class EmailProviderModel
    {
        public string Name { get; set; }

        public string LimitNumber { get; set; } 

        public string Host { get; set; }

        public string Port { get; set; }

        public bool IsDeleted { get; set; } 
        public bool IsActive { get; set; }
    }
}
