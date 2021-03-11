using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using elasticsearch_aspnet_blazor.Model;
using Microsoft.AspNetCore.Components;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            //Get quotes from url. Kudos to JamesFT and his effort to compile the quotes
            using var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("https://raw.githubusercontent.com/JamesFT/Database-Quotes-JSON/master/quotes.json");

            //Converto json using the new build in System.Text.JsonSerializer
            var quotes = JsonSerializer.Deserialize<List<QuotesModel>>(json);
            
            // Deletes existing index
            await ElasticSearchClient.DeleteIndexAsync();

            // Creates the Index, if neccessary:
            await ElasticSearchClient.CreateIndexAsync();

      

            //Insert quotes
            await ElasticSearchClient.BulkInsertAsync(quotes);


        }





    }
}