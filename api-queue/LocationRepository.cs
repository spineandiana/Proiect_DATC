using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using Newtonsoft.Json;

namespace WebAPILocatii
{
    public class LocationRepository : ILocationRepository
    {
        private string _connectionString;

        private CloudTableClient _tableClient;

        private CloudTable _locationsTable;

        public LocationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageAccountConnectionString").ToString();

            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }

        public async Task<List<Location>> GetAllLocations()
        {
            var locations = new List<Location>();

            TableQuery<Location> query = new TableQuery<Location>(); 

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Location> resultSegment = await _locationsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                locations.AddRange(resultSegment.Results);

            } while (token != null);

            return locations;
        }

        public async Task<Location> GetLocation(string id)
        {
            var parsedId = ParseLocationId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var query = TableOperation.Retrieve<Location>(partitionKey, rowKey);

            var result = await _locationsTable.ExecuteAsync(query);

            return (Location)result.Result;
        }

        public async Task InsertNewLocation(Location location)
        {
            // var insertOperation = TableOperation.Insert(location);

            // await _locationsTable.ExecuteAsync(insertOperation);

            var jsonLocationt = JsonConvert.SerializeObject(location);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonLocationt);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(
                _connectionString,
                "locations-queue"
                );
            queueClient.CreateIfNotExists();

            await queueClient.SendMessageAsync(base64String);
        }

        public async Task DeleteLocation(string id)
        {
            var parsedId = ParseLocationId(id);

            var partitionKey = parsedId.Item1;
            var rowKey = parsedId.Item2;

            var entity = new DynamicTableEntity(partitionKey, rowKey) { ETag = "*" };

            await _locationsTable.ExecuteAsync(TableOperation.Delete(entity));
        }

        public async Task EditLocation(Location location)
        {
            var editOperation = TableOperation.Merge(location);

            // Implemented using optimistic concurrency
            try
            {
                await _locationsTable.ExecuteAsync(editOperation);
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed)
                    throw new System.Exception("Entitatea a fost deja modificata. Te rog sa reincarci entitatea!");
            }
        }

        private async Task InitializeTable()
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _locationsTable = _tableClient.GetTableReference("locations");

            await _locationsTable.CreateIfNotExistsAsync();

        }

        // Used for extracting PartitionKey and RowKey from location id, assuming that id's format is "PartitionKey-RowKey", e.g "21.1546-12.6725"
        private (string, string) ParseLocationId(string id)
        {
            var elements = id.Split('-');

            return (elements[0], elements[1]);
        }

        public Task<List<Location>> GetAllLocation()
        {
            throw new System.NotImplementedException();
        }

        public Task<Location> GetLocations(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task InsertNewLocations(Location location)
        {
            throw new System.NotImplementedException();
        }

        public Task EditLocations(Location location)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteLocations(string id)
        {
            throw new System.NotImplementedException();
        }

        /*
        public Task InsertNewLocation(Location location)
        {
            throw new System.NotImplementedException();
        }*/

    }
}