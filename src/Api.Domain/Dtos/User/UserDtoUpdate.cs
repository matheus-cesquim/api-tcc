using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.User
{
    public class UserDtoUpdate
    {
        [Required (ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required (ErrorMessage = "Name is required")]
        [StringLength(30, ErrorMessage = "Name is too long")]
        public string UserName { get; set; }

        [Required (ErrorMessage = "Email is required")]
        [EmailAddress (ErrorMessage = "Email is invalid")]
        [StringLength(100 , ErrorMessage = "Email is too long")]
        public string Email { get; set; }    

        [Required (ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Password is too long")] 
        public string Password { get; set; }   
    }
}