using System.ComponentModel.DataAnnotations.Schema;
using System;
using Domain.Entities.IdentityModule;

namespace Domain.Entities.Common
{
    public class AuditableEntity
    {
        [ForeignKey("CreatedByUser")]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ModifiedByUser")]
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual UserInfo CreatedByUser { get; set; }
        public virtual UserInfo ModifiedByUser { get; set; }
    }
}