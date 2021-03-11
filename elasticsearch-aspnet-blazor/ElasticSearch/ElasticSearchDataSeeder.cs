using System;
using System.Threading.Tasks;
using elasticsearch_aspnet_blazor.ElasticSearch.Model;
using Microsoft.AspNetCore.Components;

namespace elasticsearch_aspnet_blazor.ElasticSearch
{
    public interface IElasticSearchDataSeeder
    {
        Task SeedAsync();
    }

    public class ElasticSearchDataSeeder : IElasticSearchDataSeeder
    {
        public IElasticSearchClient ElasticSearchClient { get; set; }

        public ElasticSearchDataSeeder(IElasticSearchClient elasticSearchClient)
        {
            ElasticSearchClient = elasticSearchClient;
        }


        public async Task SeedAsync()
        {
      

            // Creates the Index, if neccessary:
          await ElasticSearchClient.CreateIndex();


        }
    }
}