using System;
using Nest;

namespace elasticsearch_aspnet_blazor.ElasticSearch.Model
{
    public class QuotesModel
    {
        [Text]
        public string Text { get; set; }

        [Text]
        public string Author { get; set; }
    }

}