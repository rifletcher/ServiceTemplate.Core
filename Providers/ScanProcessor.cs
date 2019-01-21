using Serilog;
using ServiceTemplate.Core.Extension.cs;
using ServiceTemplate.Core.Interfaces;
using ServiceTemplate.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace ServiceTemplate.Core.Providers
{
    public class ScanProcessor : IScanProcessor
    {
        private readonly ILogger _logger;

        public ScanProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public bool StartProcess()
        {
            try
            {
                var allJobs = NinjectContainer.Container.GetAll<IScanJob>().Where(x => x.Active).OrderByDescending(x => x.Priority);
                foreach (var job in allJobs)
                {
                    job.DoJob().GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                _logger.Error("Program: {Message} {@Exception}", e.InnerMessage(), e);
            }
            return true;
        }
    }
}

