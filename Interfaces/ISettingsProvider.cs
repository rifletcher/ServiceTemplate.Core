using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Interfaces
{
    public interface ISettingsProvider
    {
        string DataDogEnv { get; }
        string DataDogHost { get; }
        string Metric1 { get; }
        string Metric2 { get; }
    }
}
