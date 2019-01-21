using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Modules
{
    public static class NinjectContainer
    {
        public static StandardKernel Container { get; set; }
    }
}
