using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.General
{
    [Table("tbl_AppSetting")]
    public class AppSetting:BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
    }
}
