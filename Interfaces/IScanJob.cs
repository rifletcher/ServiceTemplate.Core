using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Interfaces
{
    public interface IScanJob
    {
        //The priority of this job, with 1 being the least priority
        int Priority { get; }
        bool Active { get; }
        Task DoJob();
    }
}
