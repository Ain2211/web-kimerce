using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Email
{
    public class EmailTemplate : BaseEntityByLong
    {
        [MaxLength(512)]
        [Required]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string FilePath { get; set; }
        public string Drive { get; set; }

        public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
    }
}
