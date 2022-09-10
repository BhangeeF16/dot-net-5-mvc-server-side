using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Common.ViewModels.General
{
    public class AuditableEntityDto
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}