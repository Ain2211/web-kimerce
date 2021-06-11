using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Email
{
    public class EmailAccount : BaseEntityByLong
    {
        [MaxLength(512)]
        [Required]
        public string Email { get; set; } = "";

        [MaxLength(1024)]
        public string DisplayName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public new bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; }

        [ForeignKey("EmailProvider")]
        public long ProviderId { get; set; }
        public virtual EmailProvider Provider { get; set; } = new EmailProvider();

        public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
    }
}
