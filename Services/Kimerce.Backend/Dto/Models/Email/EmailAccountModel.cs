using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Email;

namespace Kimerce.Backend.Dto.Models.Email
{
    public class EmailAccountModel
    {
        public string Email { get; set; } 
        public string DisplayName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public long ProviderId { get; set; }
        public EmailProvider Provider { get; set; }
        public bool IsDeleted { get; set; } 
        public bool IsActive { get; set; }
    }
}
