using Newtonsoft.Json;

namespace MarketingEmailSystem.Application.DTO.Email
{
    public class SendGridErrorResponse
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }
    }

    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("help")]
        public string Help { get; set; }
    }
}