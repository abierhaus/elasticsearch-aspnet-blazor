using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;
using elasticsearch_aspnet_blazor.ElasticSearch.Model;
using Microsoft.Extensions.Configuration;
using Nest;

namespace elasticsearch_aspnet_blazor.ElasticSearch
{
    public interface IElasticSearchClient
    {
        Task<CreateIndexResponse> CreateIndex();
        BulkResponse BulkInsert(IEnumerable<QuotesModel> entities);
        Task<bool> TestConnectionAsync();
    }

    public class ElasticSearchClient : IElasticSearchClient
    {
        protected readonly IElasticClient Client;
        public readonly string IndexName = "Quotes_data";


        public ElasticSearchClient(IConfiguration configuration)
        {
            Configuration = configuration;

            var credentials = new BasicAuthenticationCredentials(Configuration["ElasticSearch:Username"]
                , Configuration["ElasticSearch:Password"]
            );


            Client = new ElasticClient(Configuration["ElasticSearch:CloudId"], credentials);
        }

        public IConfiguration Configuration { get; }

        public async Task<bool> TestConnectionAsync()
        {
            var response = await Client.PingAsync();

            return response.IsValid;
        }


        public async Task<CreateIndexResponse> CreateIndex()
        {
            var response = await Client.Indices.ExistsAsync(IndexName);
            if (response.Exists) return null;
            return await Client.Indices.CreateAsync(IndexName, index => index.Map<QuotesModel>(ms => ms.AutoMap()));
        }

        public BulkResponse BulkInsert(IEnumerable<QuotesModel> entities)
        {
            var request = new BulkDescriptor();

            foreach (var entity in entities)
                request
                    .Index<QuotesModel>(op => op
                        .Id(Guid.NewGuid().ToString())
                        .Index(IndexName)
                        .Document(entity));

            return Client.Bulk(request);
        }
    }
}