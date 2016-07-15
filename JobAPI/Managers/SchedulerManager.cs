using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAPI.Managers
{
    public class SchedulerManager
    {
        public static void Start()
        {
            HostScheduler.Start();
        }

        public static void Stop()
        {
            HostScheduler.Stop();
        }

    }
}
