using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppLocatii
{
    public class Location
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Range { get; set; }
        public string Date { get; set; }
    }
}
