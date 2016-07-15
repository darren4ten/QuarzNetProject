using JobAPI.Managers;
using JobAPI.Model;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.Read();
        }

        static void Test()
        {
            JobInfo jInfo = new JobInfo()
            {
                AssemblyName = "JobEntities",
                FileName = @"D:\MyProjects\Test\QuartzNetProject\JobEntities\bin\Debug\JobEntities.dll",
                StartTime = DateTime.Now.AddMinutes(-1),
                JobName = "testJob",
                CanConcurrent = true,
                ClassName = "JobEntities.DemoJob",
                CycleUnit = CycleUnitEnum.Second,
                GroupName = "test",
                Interval = 3,
                RepeatMode = RepeatModeEnum.Interval
            };
            var list = HostScheduler.GetCurrentScheduler().GetJobGroupNames();
            Console.WriteLine(string.Join(",", list));
            JobManager.AddJob(jInfo);
            //JobManager.DeleteJob("testJob", "test");
            HostScheduler.Start();
        }
    }
}
