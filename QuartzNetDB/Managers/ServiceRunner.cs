using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAPI.Managers
{
    public class ServiceRunner
    {
        public void Start()
        {
            SchedulerManager.Start();
        }

        public void Stop()
        {
            SchedulerManager.Stop();
        }
    }
}
