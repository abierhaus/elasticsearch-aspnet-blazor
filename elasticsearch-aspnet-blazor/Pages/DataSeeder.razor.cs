using System.Threading.Tasks;
using elasticsearch_aspnet_blazor.ElasticSearch;
using Microsoft.AspNetCore.Components;

namespace elasticsearch_aspnet_blazor.Pages
{
    public partial class DataSeeder
    {
        [Inject]
        public IElasticSearchDataSeeder ElasticSearchDataSeeder { get; set; }


        private async Task SeedData()
        {
           await ElasticSearchDataSeeder.SeedAsync();
        }
    }
}