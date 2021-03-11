using System.Text.Json.Serialization;
using Nest;

namespace elasticsearch_aspnet_blazor.Model
{
    public class QuotesModel
    {
        [JsonPropertyName("quoteText")]
        [Keyword]
        public string Text { get; set; }

        
        [JsonPropertyName("quoteAuthor")]
        [Keyword]
        public string Author { get; set; }
    }

}