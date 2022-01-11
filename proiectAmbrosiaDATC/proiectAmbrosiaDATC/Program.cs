using Microsoft.Azure.Cosmos.Table;
using StorageTable;
using System;
using System.Threading.Tasks;

namespace proiectAmbrosiaDATC
{
    class Program
    {
        static string storageconn = "DefaultEndpointsProtocol=https;AccountName=proiectdatc;AccountKey=8fmZm7LwNqQMrAcXkD6ca1EA55PjNYyAUjzrr4Bhm/aWQ0H9e0Id9YNuD5ZiHfaxzpEPq73n5wnAX+4cNVJooA==;EndpointSuffix=core.windows.net";
        static string table1 = "locatiiconf";
        static string table2 = "locatiineconf";
        static string partitionkey = "Latitude";
        static string rowKey = "Longitude";

        static void Main(string[] args)
        {
            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageconn);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tblclient.GetTableReference(table1);

            //CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
           CloudTable table3 = tblclient.GetTableReference(table2);

            InsertTableEntity(table).Wait();

            InsertTableEntity(table3).Wait();
            //InsertBatch(table).Wait();

            //Query(table).Wait();
            //ReadEntity(table, partitionkey, rowKey).Wait();

            Console.ReadKey();
        }

        public static async Task<string> Query(CloudTable p_tbl)
        {

            TableQuery<Location> CustomerQuery = new TableQuery<Location>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Swapan Sharma"
                ));
            var itemlist = p_tbl.ExecuteQuery(CustomerQuery);
            foreach (Location obj in itemlist)
            {
                Console.WriteLine("Latitude: " + obj.PartitionKey);
                Console.WriteLine("Longitude: " + obj.RowKey);
                Console.WriteLine("Range: " + obj.Range);
                Console.WriteLine("Date: " + obj.Date);
            }
            return "Operation complete";
        }

        public static async Task<string> InsertBatch(CloudTable p_tbl)
        {
            TableBatchOperation l_batch = new TableBatchOperation();
            // All of the records should have the same partition key
            Location entity1 = new Location("45.4500", "21.1355");
            entity1.Range = 35000;
            Location entity2 = new Location("50.4500", "21.1355");
            entity2.Range = 20000;
            Location entity3 = new Location("55.4500", "21.1355");
            entity3.Range = 30000;
            l_batch.Insert(entity1);
            l_batch.Insert(entity2);
            l_batch.Insert(entity3);
            p_tbl.ExecuteBatch(l_batch);
            Console.WriteLine("Records Inserted");
            return "Completed";
        }

        public static async Task<string> InsertTableEntity(CloudTable p_tbl)
        {
            //parcul rozelor
            Location entity = new Location("45.4500", "21.1355");
            entity.Range = 20;
            entity.Date = "06.01.2022";
            TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
            TableResult result = await p_tbl.ExecuteAsync(insertOperation);

            //parcul botanic
            Location entity0 = new Location("45.70655", "21.22548");
            entity0.Range = 12;
            entity0.Date = "02.01.2022";
            TableOperation insertOperation0 = TableOperation.InsertOrMerge(entity0);
            TableResult result0 = await p_tbl.ExecuteAsync(insertOperation0);

            Location entity1 = new Location("44.84213", "21.56754");
            entity1.Range = 50;
            entity1.Date = "10.01.2022";
            TableOperation insertOperation1 = TableOperation.InsertOrMerge(entity1);
            TableResult result1 = await p_tbl.ExecuteAsync(insertOperation1);

            Location entity2 = new Location("45.4500", "21.1355");
            entity2.Range = 30;
            entity2.Date = "09.01.2022";
            TableOperation insertOperation2 = TableOperation.InsertOrMerge(entity2);
            TableResult result2 = await p_tbl.ExecuteAsync(insertOperation2);

            Location entity3 = new Location("47.4500", "21.71355");
            entity3.Range = 35;
            entity3.Date = "05.01.2022";
            TableOperation insertOperation3 = TableOperation.InsertOrMerge(entity3);
            TableResult result3 = await p_tbl.ExecuteAsync(insertOperation3);

            Location entity4 = new Location("46.49560", "21.12255");
            entity4.Range = 10;
            entity4.Date = "06.11.2021";
            TableOperation insertOperation4 = TableOperation.InsertOrMerge(entity4);
            TableResult result4 = await p_tbl.ExecuteAsync(insertOperation4);

            Location entity5 = new Location("38.49560", "20.12255");
            entity5.Range = 22;
            entity5.Date = "17.12.2021";
            TableOperation insertOperation5 = TableOperation.InsertOrMerge(entity5);
            TableResult result5 = await p_tbl.ExecuteAsync(insertOperation5);

            Location entity6 = new Location("47.45678", "18.12255");
            entity6.Range = 33;
            entity6.Date = "04.12.2021";
            TableOperation insertOperation6 = TableOperation.InsertOrMerge(entity6);
            TableResult result6 = await p_tbl.ExecuteAsync(insertOperation6);

            Location entity7 = new Location("60.8960", "32.12255");
            entity7.Range = 15;
            entity7.Date = "24.12.2021";
            TableOperation insertOperation7 = TableOperation.InsertOrMerge(entity7);
            TableResult result7 = await p_tbl.ExecuteAsync(insertOperation7);

            Location entity8 = new Location("51.49560", "19.45783");
            entity8.Range = 27;
            entity8.Date = "25.12.2021";
            TableOperation insertOperation8 = TableOperation.InsertOrMerge(entity8);
            TableResult result8 = await p_tbl.ExecuteAsync(insertOperation8);

            Location entity9 = new Location("47.49560", "19.12255");
            entity9.Range = 17;
            entity9.Date = "18.12.2021";
            TableOperation insertOperation9 = TableOperation.InsertOrMerge(entity9);
            TableResult result9 = await p_tbl.ExecuteAsync(insertOperation9);

            Location entity10 = new Location("48.11160", "21.11354");
            entity10.Range = 18;
            entity10.Date = "31.12.2021";
            TableOperation insertOperation10 = TableOperation.InsertOrMerge(entity10);
            TableResult result10 = await p_tbl.ExecuteAsync(insertOperation10);


            Console.WriteLine("Location Added");
            return "Location added";
        }


        public static async Task<string> ReadEntity(CloudTable p_tbl, string p_PartitionKey, string p_RowKey)
        {
            TableOperation readOperation = TableOperation.Retrieve<Location>(p_PartitionKey, p_RowKey);
            TableResult result = await p_tbl.ExecuteAsync(readOperation);

            Location obj = result.Result as Location;
            Console.WriteLine("Latitude: " + obj.PartitionKey);
            Console.WriteLine("Longitude: " + obj.RowKey);
            Console.WriteLine("Range: " + obj.Range);
            Console.WriteLine("Date: " + obj.Date);

            return "Entity read operation complete";
        }
    }
}
