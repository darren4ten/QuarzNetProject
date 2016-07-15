using JobAPI.Managers;
using JobAPI.Model;
using Quartz;
using Quartz.Impl;
using QuartzNetProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace JobAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            Console.Read();
        }

        static void Run()
        {
            //HostFactory.Run(x =>                                 //1
            //{
            //    x.Service<TownCrier>(s =>                        //2
            //    {
            //        s.ConstructUsing(name => new TownCrier());     //配置一个完全定制的服务,对Topshelf没有依赖关系。常用的方式。
            //        //the start and stop methods for the service
            //        s.WhenStarted(tc => tc.Start());              //4
            //        s.WhenStopped(tc => tc.Stop());               //5
            //    });
            //    x.RunAsLocalSystem();                            // 服务使用NETWORK_SERVICE内置帐户运行。身份标识，有好几种方式，如：x.RunAs("username", "password");  x.RunAsPrompt(); x.RunAsNetworkService(); 等

            //    x.SetDescription("Sample Topshelf Host服务的描述");        //安装服务后，服务的描述
            //    x.SetDisplayName("Stuff显示名称");                       //显示名称
            //    x.SetServiceName("Stuff服务名称");                       //服务名称
            //});    

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();

                x.Service<ServiceRunner>();
                x.RunAsLocalService();
                x.SetDescription("QuartzDemo服务描述");
                x.SetDisplayName("QuartzDemo服务");
                x.SetServiceName("QuartzDemo服务名称");

                x.EnablePauseAndContinue();
            });
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
