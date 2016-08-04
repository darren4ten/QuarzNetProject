using JobAPI.Managers;
using JobHost;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace QuartzNetProject
{
    public class ServiceRunner : ServiceControl, ServiceSuspend
    {


        public ServiceRunner()
        {

        }

        public bool Start(HostControl hostControl)
        {
            JobHostDomain domain = new JobHostDomain();
            domain.Run();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            SchedulerManager.Stop();
            return true;
        }

        //public bool Continue(HostControl hostControl)
        //{
        //    scheduler.ResumeAll();
        //    return true;
        //}

        //public bool Pause(HostControl hostControl)
        //{
        //    scheduler.PauseAll();
        //    return true;
        //}

        public bool Continue(HostControl hostControl)
        {
            throw new NotImplementedException();
        }

        public bool Pause(HostControl hostControl)
        {
            throw new NotImplementedException();
        }
    }
}
