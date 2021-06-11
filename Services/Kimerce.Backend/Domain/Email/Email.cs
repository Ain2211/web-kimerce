using Kimerce.Backend.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Email
{
    public class Email : BaseEntityByLong
    {
        [Required]
        [MaxLength(1024)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(1024)]
        public string From { get; set; }
        [Required]
        [MaxLength(1024)]
        public string To { get; set; }
        [Required]
        public string Body { get; set; }

        [ForeignKey("EmailTemplate")]
        public long TemplateId { get; set; }
        public virtual EmailTemplate Template { get; set; }

        [ForeignKey("EmailAccount")]
        public long AccountId { get; set; }
        public virtual EmailAccount Account { get; set; }
    }
}
