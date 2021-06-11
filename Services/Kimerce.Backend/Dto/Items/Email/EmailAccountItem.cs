using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Email;

namespace Kimerce.Backend.Dto.Items.Email
{
    public class EmailAccountItem
    {
        public long Id { get; set; }
        public string Email { get; set; } 
        public string DisplayName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public long ProviderId { get; set; }
    }
}
