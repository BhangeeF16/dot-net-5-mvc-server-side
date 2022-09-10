using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    public class BaseForUser : IdentityUser
    {
        [ForeignKey("CreatedByUser")]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ModifiedByUser")]
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public virtual UserInfo CreatedByUser { get; set; }
        public virtual UserInfo ModifiedByUser { get; set; }

    }
}
