using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace DTOGenerator
{
    public class DTOGenerator
    {
        public DTOGenerator()
        {
            int threadPoolLimit;
            if (Int32.TryParse(ConfigurationManager.AppSettings.Get("threadPoolLimit"), out threadPoolLimit))
            {
                ThreadPool.SetMaxThreads(threadPoolLimit, 1);
            }
        }

        public static void MakeClass()
        {
            
        }
    }
}
