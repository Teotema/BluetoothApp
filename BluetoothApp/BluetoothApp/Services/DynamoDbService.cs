using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using BluetoothApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.IotData;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.IotData.Model;
using System.IO;

namespace BluetoothApp.Services
{
    internal class DynamoDbService : IDynamoDbService
    {
        private class PresureDto
        {
            public long Id { get; set; }
            public DateTime Start { get; set; }

            public DateTime End { get; set; }

            public List<uint> Presures { get; set; }

        }
        private readonly AmazonIotDataClient _client;
        public DynamoDbService()
        {
            AWSCredentials credentials =
            new BasicAWSCredentials("AKIAUOMXFRQA7VTVSUHC", "7hsN9YIkHwpi7fgMU4pLDb+mgDDEzxRo4wh54nDG");
            RegionEndpoint endpoint = RegionEndpoint.USEast1;
            _client = new AmazonIotDataClient("https://a2ov6at9v9zdj2-ats.iot.us-east-1.amazonaws.com/", credentials);;
            
        }

        public async Task AddPreassureMeasurement(PreassureMeasurement measurement)
        {
            PresureDto presureDto = new PresureDto()
            {
                Id = DateTime.Now.Ticks,
                Start = measurement.Start,
                End = measurement.End,
                Presures = measurement.Preassures
            };

           

            var js = JsonConvert.SerializeObject(presureDto);
            
            var payloadStream = new MemoryStream(Encoding.UTF8.GetBytes(js ?? string.Empty));

            PublishRequest publishRequest = new PublishRequest()
            {
                Topic = "iot/topic",
                Qos = 0,
                Payload = payloadStream
            };
            await _client.PublishAsync(publishRequest);
        }
      
    }
}
