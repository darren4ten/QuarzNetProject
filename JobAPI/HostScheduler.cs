using JobAPI.Managers;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAPI
{
    public class HostScheduler : IDisposable
    {
        private static readonly IScheduler scheduler = null;
        static HostScheduler()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.ListenerManager.AddSchedulerListener(new HostSchedulerListener());
        }

        public static IScheduler GetCurrentScheduler()
        {
            return scheduler;
        }

        /// <summary>
        /// 新增job
        /// </summary>
        /// <param name="job"></param>
        /// <param name="trigger"></param>
        internal static void RegisterJob(IJobDetail job, ITrigger trigger)
        {
            scheduler.DeleteJob(job.Key);
            scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// 删除job
        /// </summary>
        internal static void RemoveJob(JobKey jobKey)
        {
            scheduler.DeleteJob(jobKey);
        }

        public static void Start()
        {
            scheduler.Start();
            Console.WriteLine("-----Start----");
        }

        public static void Stop()
        {
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
                Console.WriteLine("-----Shutdown----");
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
