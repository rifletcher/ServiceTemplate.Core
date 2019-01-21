using Ninject.Modules;
using Serilog;
using ServiceTemplate.Core.Interfaces;
using ServiceTemplate.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Modules
{
    class ServiceModule : NinjectModule
    {
        private readonly ILogger _logger;

        public ServiceModule(ILogger logger)
        {
            _logger = logger;
        }
        public override void Load()
        {
            //setup logging
            this.Bind<ILogger>().ToConstant(_logger);
            // General
            this.Bind<ISettingsProvider>().To<SettingsProvider>();
            this.Bind<IScanProcessor>().To<ScanProcessor>();
            this.Bind<IScanJob>().To<DemoProvider>();
        }
    }
}
