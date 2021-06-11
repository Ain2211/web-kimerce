using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Images
{
    public class ImageModel : BaseModelInt
    {
         public string name { get; set; }

        public string path { get; set; }
        public string uri { get; set; }
        public string domain { get; set; }
        public string driver { get; set; }
    }
}
