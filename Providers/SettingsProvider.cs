using ServiceTemplate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Providers
{
    public class SettingsProvider : ISettingsProvider
    {
        private T Get<T>(string name)
        {
            var configurationAppSettings = new System.Configuration.AppSettingsReader();
            return (T)configurationAppSettings.GetValue(name, typeof(T));
        }

        public string DataDogEnv => Get<string>("DataDogEnv");

        public string DataDogHost => Get<string>("DataDogHost");

        public string Metric1 => Get<string>("Metric1");

        public string Metric2 => Get<string>("Metric2");
    }
}
