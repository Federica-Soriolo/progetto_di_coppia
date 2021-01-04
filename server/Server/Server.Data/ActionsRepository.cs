using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Server.Models;
using System;
using System.Threading.Tasks;

namespace Server.Data
{
    public class ActionsRepository: IActionsRepository
    {
        private readonly IConfiguration _configuration;
        public ActionsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task TableServiceAsync(string deviceId, DataModel data)
        {

            var storageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(_configuration.GetConnectionString("Storage"));
            var tableMessage = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var messageClient = tableMessage.GetTableReference("progettovalle");
            await messageClient.CreateIfNotExistsAsync();

            TableModel t = new TableModel(Guid.NewGuid(), deviceId, data.Speed, data.Battery, data.Latitude, data.Longitude);

            var insertFile = TableOperation.Insert(t);
            var resultInsert = await messageClient.ExecuteAsync(insertFile);

        }


    }
}

