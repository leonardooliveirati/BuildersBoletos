using Newtonsoft.Json;

namespace BuildersBoleto.Domain.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}