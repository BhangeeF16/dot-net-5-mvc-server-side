using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities.IdentityModule
{
    public class Role : IdentityRole
    {
        public bool IsActive { get; set; } = true;

        public Role()
        {
        }

        public Role(string Name) : base(Name)
        {

        }
    }
}
