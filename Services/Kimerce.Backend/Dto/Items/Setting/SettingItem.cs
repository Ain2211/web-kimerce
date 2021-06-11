using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Setting
{
    public class SettingItem
    {
        public int Id { get; set; }
        public string SystemName { get; set; } 
        public string Name { get; set; } 
        public string Value { get; set; }
    }
}
