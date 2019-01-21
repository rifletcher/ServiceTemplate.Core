using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTemplate.Core.Extension.cs
{
    public static class ExceptionExts
    {
        public static Exception InnerMostException(this Exception e)
        {
            if (e == null)
                return null;

            while (e.InnerException != null)
                e = e.InnerException;

            return e;
        }

        public static string InnerMessage(this Exception e)
        {
            return InnerMostException(e)?.Message;
        }
    }
}

