using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class LoginDtoResult
    {
        public bool Authenticated { get; set; }
        public string Message { get; set; }
        public string? Created { get; set; }
        public string? Expiration { get; set; }
        public string? AccessToken { get; set; }
        public string? Email { get; set; }
    }
}