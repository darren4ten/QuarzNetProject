using Quartz;
using Quartz.Impl;
using JobAPI.Managers;
using JobAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobAPI
{
    class Program
    {
        public class HelloJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                Console.WriteLine("Greetings from HelloJob!");
            }
        }
        static void Main(string[] args)
        {
            Demo();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }

        static void Demo()
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
            //JobManager.AddJob(jInfo);
            //JobManager.DeleteJob("testJob", "test");
            HostScheduler.Start();
        }

        static void Test()
        {
            try
            {
                Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };

                // Grab the Scheduler instance from the Factory 
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // and start it off
                scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1").RequestRecovery(true)
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger

                scheduler.ScheduleJob(job, trigger);
                // scheduler.AddJob(job, true);

                // some sleep to show what's happening
                Thread.Sleep(TimeSpan.FromSeconds(60));

                // and last shut down the scheduler when you are ready to close your program
                scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }
    }
}
