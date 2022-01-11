using Microsoft.WindowsAzure.Storage.Table;

namespace Models
{
    public class Location : TableEntity
    {
        public Location(string Latitude, string Longitude)
        {
            this.PartitionKey = Latitude;
            this.RowKey = Longitude;
        }

        public Location() { }

        public int Range { get; set; }

        public string Date { get; set; }

    }
}