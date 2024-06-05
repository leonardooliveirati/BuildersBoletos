using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildersBoleto.Domain.Models
{
    public class BoletoApiResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("due_date")]
        public string DueDate { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("recipient_name")]
        public string RecipientName { get; set; }
        [JsonProperty("recipient_document")]
        public string RecipientDocument { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
