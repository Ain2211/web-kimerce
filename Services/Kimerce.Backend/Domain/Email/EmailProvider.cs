using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Kimerce.Backend.Domain.Email
{
    public class EmailProvider : BaseEntityByLong
    {
        [MaxLength(256)]
        [Required]
        public string Name { get; set; } = "";

        [MaxLength(512)]
        [Required]
        public string LimitNumber { get; set; } = "";

        [MaxLength(256)]
        [Required]
        public string Host { get; set; } = "";

        [MaxLength(256)]
        [Required]
        public string Port { get; set; } = "";

        public new bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; }

        public virtual ICollection<EmailAccount> Accounts { get; set; } = new List<EmailAccount>();
    }
}
