using Kimerce.Backend.Domain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Email
{
    public class EmailItem
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public long TemplateId { get; set; }
        public long AccountId { get; set; }
    }
}
