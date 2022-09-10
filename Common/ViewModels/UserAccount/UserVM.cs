using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.ViewModels.UserAccount
{
    public class UserVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public bool Active { get; set; }
        public string Gender { get; set; }
        public string ActivationKey { get; set; }
        public List<string> Roles { get; set; }
        public string DriverLicense { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public string Address { get; set; }
    }

    public class UserSignupVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
    }
}