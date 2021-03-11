using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using elasticsearch_aspnet_blazor.Model;
using Microsoft.Extensions.Configuration;
using Nest;

namespace elasticsearch_aspnet_blazor.ElasticSearch
{
    public interface IElasticSearchClient
    {
        Task<CreateIndexResponse> CreateIndexAsync();
        Task BulkInsertAsync(IEnumerable<QuotesModel> entities);
        Task<bool> TestConnectionAsync();
        Task<IReadOnlyCollection<QuotesModel>> GetQuotesByAuthorAsync(string author);
        Task DeleteIndexAsync();
    }

    public class ElasticSearchClient : IElasticSearchClient
    {
        protected readonly IElasticClient Client;
        public readonly string IndexName = "quotes";


        public ElasticSearchClient(IConfiguration configuration)
        {

            //Configure client with credentials. Note: We will also use the default indexname here

            var settings = new ConnectionSettings(new Uri(configuration["ElasticSearch:Uri"]))
                .DefaultIndex(IndexName).BasicAuthentication(configuration["ElasticSearch:Username"]
                    , configuration["ElasticSearch:Password"]).EnableDebugMode();


            Client = new ElasticClient(settings);
        }


        public async Task<bool> TestConnectionAsync()
        {
            var response = await Client.PingAsync();

            return response.IsValid;
        }

        /// <summary>
        ///     Get the quotes by the name of the author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<QuotesModel>> GetQuotesByAuthorAsync(string author)
        {
            var searchResponse = await Client.SearchAsync<QuotesModel>(s => s
                .From(0)
                .Size(25)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Author)
                        .Query(author)
                    )
                )
            );

            //Return documents from query
            var documents = searchResponse.Documents;
            return documents;
        }

        /// <summary>
        ///     Create an index
        /// </summary>
        /// <returns></returns>
        public async Task<CreateIndexResponse> CreateIndexAsync()
        {
            var response = await Client.Indices.ExistsAsync(IndexName);
            if (response.Exists) return null;


            return await Client.Indices.CreateAsync(IndexName, index => index.Map<QuotesModel>(ms => ms.AutoMap()));
        }

        /// <summary>
        ///     Deletes the index e.g. for clean up
        /// </summary>
        /// <returns></returns>
        public async Task DeleteIndexAsync()
        {
            await Client.Indices.DeleteAsync(IndexName);
        }

        /// <summary>
        ///     Inserts/ Index documents
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<QuotesModel> entities)
        {
            var request = new BulkDescriptor();


            foreach (var entity in entities)
                request
                    .Index<QuotesModel>(op => op
                        .Id(Guid.NewGuid().ToString())
                        .Document(entity));
            await Client.BulkAsync(request);
        }
    }
}