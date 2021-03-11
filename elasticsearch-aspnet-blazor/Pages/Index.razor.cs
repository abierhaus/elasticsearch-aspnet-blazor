using System.Threading.Tasks;
using elasticsearch_aspnet_blazor.ElasticSearch;
using Microsoft.AspNetCore.Components;

namespace elasticsearch_aspnet_blazor.Pages
{
    public partial class Index
    {
        [Inject]
        public IElasticSearchClient ElasticSearchClient { get; set; }
        

        public bool IsClientWorking { get; set; }
        protected override async Task OnInitializedAsync()
        {
            IsClientWorking = await ElasticSearchClient.TestConnectionAsync();

        }

    }
}