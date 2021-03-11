using System.Text.Json.Serialization;
using Nest;

namespace elasticsearch_aspnet_blazor.Model
{
    public class QuotesModel
    {
        
        [JsonPropertyName("quoteText")] //Attribute name in json
        [Keyword] //Attribute for ElasticSearch
        public string Text { get; set; }

        
        [JsonPropertyName("quoteAuthor")] //Attribute name in json
        [Keyword] //Attribute for ElasticSearch
        public string Author { get; set; }
    }

}