using System.Collections.Generic;
using System.ComponentModel;
using Kimerce.Backend.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;


namespace Kimerce.Backend.Domain.Setting
{
    public class Setting : BaseEntityByInt
    {

        [MaxLength(512)]
        [Required]
        public string SystemName { get; set; } = "";
        [MaxLength(1024)]
        [Required]
        public string Name { get; set; } = "";
        [MaxLength(1024)]
        [Required]
        public string Value { get; set; } = "";
    }
}
