using Ninject;
using Serilog;
using Serilog.Formatting.Json;
using ServiceTemplate.Core.Interfaces;
using ServiceTemplate.Core.Modules;
using StatsdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServiceTemplate.Core
{
    class Service
    {
        private readonly Timer _timer;
        private static readonly object ThreadLock = new object();
        private readonly IScanProcessor _scanProcessor;

        protected static int DelayTime
        {
            get
            {
                var configurationAppSettings = new System.Configuration.AppSettingsReader();
                return (int)configurationAppSettings.GetValue("DelayTime", typeof(int));
            }
        }

        protected static string LogFile
        {
            get
            {
                var configurationAppSettings = new System.Configuration.AppSettingsReader();
                return (string)configurationAppSettings.GetValue("LogFile", typeof(string));
            }
        }

        private readonly ILogger appLogger;

        private void ProcessScan(object source, ElapsedEventArgs e)
        {
            lock (ThreadLock)
            {
                _timer.Enabled = false;
                try
                {
                    _scanProcessor.StartProcess();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
                finally
                {
                    _timer.Enabled = true;
                }
            }
        }

        public Service()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.ColoredConsole().WriteTo.RollingFile(new JsonFormatter(), LogFile, shared: true).CreateLogger();
            try
            {
                var dogstatsdConfig = new StatsdConfig
                {
                    StatsdServerName = "127.0.0.1",
                    StatsdPort = 8125, // Optional; default is 8125
                    Prefix = Constants.DataDogTagPrefix // Optional; by default no prefix will be prepended
                };
                DogStatsd.Configure(dogstatsdConfig);

                appLogger = Log.Logger;
                appLogger.Information("ServiceTemplate.Core startup");
                NinjectContainer.Container = new StandardKernel(new ServiceModule(appLogger));
                _scanProcessor = NinjectContainer.Container.Get<IScanProcessor>();
                _timer = new Timer(DelayTime);
                _timer.Elapsed += ProcessScan;
                _timer.AutoReset = true;
                ProcessScan(null, null); // kickoff the inital one so no need to wait.
                _timer.Enabled = true;
            }
            catch (Exception e)
            {
                appLogger.Fatal("Program: {@Exception}", e);
                Environment.Exit(1);
            }
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        // service stop
        public void Stop()
        {
            _timer.Enabled = false;
        }
    }
}
