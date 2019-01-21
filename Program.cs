using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace ServiceTemplate.Core
{
    class Program
    {
        protected static string ServiceName
        {
            get
            {
                var configurationAppSettings = new System.Configuration.AppSettingsReader();
                return (string)configurationAppSettings.GetValue("ServiceName", typeof(string));
            }
        }

        protected static string DisplayName
        {
            get
            {
                var configurationAppSettings = new System.Configuration.AppSettingsReader();
                return (string)configurationAppSettings.GetValue("DisplayName", typeof(string));
            }
        }

        protected static string ServiceDescription
        {
            get
            {
                var configurationAppSettings = new System.Configuration.AppSettingsReader();
                return (string)configurationAppSettings.GetValue("ServiceDescription", typeof(string));
            }
        }

        public static int Main(string[] args)
        {

            {
                var exitCode = HostFactory.Run(c =>
                {
                    c.Service<Service>(service =>
                    {
                        c.SetServiceName(ServiceName);
                        c.SetDisplayName(DisplayName);
                        c.SetDescription(ServiceDescription);

                        c.EnablePauseAndContinue();
                        c.EnableShutdown();

                        c.StartAutomatically();
                        c.RunAsLocalSystem();

                        ServiceConfigurator<Service> s = service;
                        s.ConstructUsing(() => new Service());
                        s.WhenStarted(a => a.Start());
                        s.WhenStopped(a => a.Stop());
                    });
                });
                return (int)exitCode;
            }
        }
    }
}
