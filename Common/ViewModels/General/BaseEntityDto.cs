using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Common.ViewModels.General
{
    public class BaseEntityDto
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool? IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;

    }
}
