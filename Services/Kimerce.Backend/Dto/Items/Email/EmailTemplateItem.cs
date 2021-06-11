using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Email
{
    public class EmailTemplateItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Drive { get; set; }
    }
}
