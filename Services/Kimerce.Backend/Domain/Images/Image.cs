using Kimerce.Backend.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Images
{
    public class Image : BaseEntityInt
    {
        public Image()
        {
             this.name = "#";
            this.path = "/";
            this.uri = "google.com";
            this.domain = "google.com";
            this.driver = "C:\\";
            
        }
        public string name { get; set; }

        public string path { get; set; }
        public string uri { get; set; }
        public string domain { get; set; }
        public string driver { get; set; }


    }
}
