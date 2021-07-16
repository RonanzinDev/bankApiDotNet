using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bankApi.Models
{
    // basicamente seria o DTO para filtar alguns campos
    public class RegisterNewAccountModel
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        //cummulative
        [Required]
        [RegularExpression(@"^[0-9]{4}$")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ConfirmPin { get; set; }
    }
}