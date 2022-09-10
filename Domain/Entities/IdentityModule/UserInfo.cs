#region Imports

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Common.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Domain.Entities.Common;

#endregion

namespace Domain.Entities.IdentityModule
{
    public class UserInfo : BaseForUser
    {
        [Required]
        [MaxLength(35)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(35)]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        [MaxLength(100)]
        public string DriverLicense { get; set; }
        [MaxLength(100)]
        public string EmergencyContactName { get; set; }
        [MaxLength(100)]
        public string EmergencyContactNo { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
    }
}