using System;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppLocatii
{
    public class Function1
    {
        [FunctionName("Function1")]
        [return: Table("locatiineconf", Connection = "DatcStorage")]
        public Location Run([QueueTrigger("SBproiectDATC")] CloudQueueMessage message, ILogger log)
        {
            log.LogInformation($"New message: {message.AsString}");
            Location location = default;
            try { location = JsonConvert.DeserializeObject<Location>(message.AsString); } catch { }
            if (location is null)
                log.LogWarning($"Could not deserialize message into StudentEntity");
            return location;
        }
    }
}
