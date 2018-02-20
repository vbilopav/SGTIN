using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SgtinAppCore.Interfaces;
using SgtinAppCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SgtinInfrastructure
{
    public class SgtinDataRepository : IRepository<SgtinData>
    {
        private const string fileName = @"data\data.csv";
        private static readonly Object lockObject = new Object();
        private static readonly IList<SgtinData> data = new List<SgtinData>();
        private readonly ILogger logger;

        public IEnumerable<SgtinData> SearchBySpec(string query)
        {
            return data.Where(d => d.ItemName == query);
        }

        private void InitializeData()
        {
            using (var file = new StreamReader(fileName))
            {
                file.ReadLine(); // skip header
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        var split = line.Split(";");
                        data.Add(new SgtinData(Convert.ToUInt64(split[0]), split[1], Convert.ToUInt64(split[2]), split[3]));
                    }
                    catch (Exception exception)
                    {
                        logger.LogError(exception, $"Line couldn't be parsed. Data: {line}");
                    }                    
                }
                logger.LogInformation($"{data.Count} rows loaded from {fileName}");
            }
        }

        public SgtinDataRepository(ILogger<SgtinDataRepository> logger)
        {
            this.logger = logger;    
            lock (lockObject)
            {
                InitializeData();
            }
        }
    }
}
