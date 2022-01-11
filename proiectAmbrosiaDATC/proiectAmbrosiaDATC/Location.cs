using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table;

namespace StorageTable
{
    class Location : TableEntity
    {
        public Location() { }

        public Location(string Latitude, string Longitude)
        {
            PartitionKey = Latitude;
            RowKey = Longitude;
        }
        public int Range { get; set; }

        public string Date { get; set; }
    }
}
