using System.Collections.Generic;
using System.Threading.Tasks;
using elasticsearch_aspnet_blazor.ElasticSearch;
using elasticsearch_aspnet_blazor.Model;
using Microsoft.AspNetCore.Components;

namespace elasticsearch_aspnet_blazor.Pages
{
    public partial class Search
    {
        [Inject] public IElasticSearchClient ElasticSearchClient { get; set; }

        public IEnumerable<QuotesModel> Quotes { get; set; }


        public string SearchValue { get; set; }


        private async Task SearchQuotesAsync()
        {
            Quotes = await ElasticSearchClient.GetQuotesByAuthorAsync(SearchValue);
        }
    }
}