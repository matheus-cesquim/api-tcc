using System;
using Newtonsoft.Json;

namespace Api.Integration.Test
{
    public class LoginResponseDto
    {
        [JsonProperty("authenticated")]
        public bool Authenticated { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}