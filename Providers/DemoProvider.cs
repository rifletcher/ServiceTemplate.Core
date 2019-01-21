using Serilog;
using ServiceTemplate.Core.Interfaces;
using StatsdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Providers
{
    public class DemoProvider : IScanJob
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ILogger _logger;

        public int Priority => 1;

        public bool Active => true;

        public DemoProvider(ISettingsProvider settingsProvider, ILogger logger)
        {
            _settingsProvider = settingsProvider;
            _logger = logger;
        }

        private static string[] DataDogTags = null;

        public async Task DoJob()
        {
            _logger.Information("Demo Process");

            Random rnd = new Random();
            var metric1 = rnd.Next(1, 1000);
            var metric2 = rnd.Next(2, 1000);

            var tags = "maintainer:company,environment:{env},application:service_template_core";
            if (!string.IsNullOrWhiteSpace(tags))
            {
                tags = tags.Replace("{env}", _settingsProvider.DataDogEnv);
                tags = tags.Replace("{host}", _settingsProvider.DataDogHost);
                DataDogTags = tags.Split(',');
                _logger.Information("The values are {0} and {1}", metric1, metric2);
            }
            DogStatsd.Histogram(_settingsProvider.Metric1, metric1, tags: DataDogTags);
            DogStatsd.Histogram(_settingsProvider.Metric2, metric2, tags: DataDogTags);
        }
    }
}
