
using JobCommon;
using JobCommon.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobAPI.Managers
{
    public class JobManager
    {
        public static void AddJob(JobInfo jobInfo)
        {

            if (string.IsNullOrEmpty(jobInfo.JobName) || string.IsNullOrEmpty(jobInfo.GroupName))
            {
                throw new Exception("job名称与组名都不能为空");
            }

            if (jobInfo.StartTime < new DateTime(1900, 1, 1))
            {
                throw new Exception("开始时间不正确");
            }

            if (string.IsNullOrEmpty(jobInfo.FileName) || string.IsNullOrEmpty(jobInfo.ClassName))
            {
                throw new Exception("程序集地址和类名都不能为空");
            }

            // 反射获取job实例
            Assembly assembly = Consts.GetAssembly(jobInfo.FileName);
            var jjjjjjob = assembly.CreateInstance(jobInfo.ClassName);
            BaseJob job = assembly.CreateInstance(jobInfo.ClassName) as BaseJob;
            if (job == null)
            {
                throw new Exception("无法实例化job类");
            }

            JobBuilder jobBuilder = null;
            if (jobInfo.CanConcurrent)
            {
                jobBuilder = JobBuilder.Create<HostJob>();              // 可以多线程
            }
            else
            {
                jobBuilder = JobBuilder.Create<HostJob>();        // 不能多线程
            }

            IJobDetail innerJob = jobBuilder
                .WithIdentity(jobInfo.JobName, jobInfo.GroupName)
                .UsingJobData(Consts.JobParameter, jobInfo.Parameter)
                .UsingJobData(Consts.JobAssemblyPath, jobInfo.FileName)
                .UsingJobData(Consts.JobClassName, jobInfo.ClassName)
                .RequestRecovery(true)
                .Build();

            TriggerBuilder builder = TriggerBuilder.Create().WithIdentity(jobInfo.JobName, jobInfo.GroupName);
            if (jobInfo.RepeatMode == RepeatModeEnum.Interval)
            {
                if (jobInfo.Interval <= 0)
                {
                    throw new Exception("时间间隔必须大于0");
                }

                switch (jobInfo.CycleUnit)
                {
                    case CycleUnitEnum.Day:
                        builder = builder.WithSimpleSchedule(x => x.WithIntervalInHours(24 * jobInfo.Interval).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires());
                        break;
                    case CycleUnitEnum.Hour:
                        builder = builder.WithSimpleSchedule(x => x.WithIntervalInHours(jobInfo.Interval).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires());
                        break;
                    case CycleUnitEnum.Minute:
                        builder = builder.WithSimpleSchedule(x => x.WithIntervalInMinutes(jobInfo.Interval).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires());
                        break;
                    case CycleUnitEnum.Second:
                        builder = builder.WithSimpleSchedule(x => x.WithIntervalInSeconds(jobInfo.Interval).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires());
                        break;
                }
            }
            else if (jobInfo.RepeatMode == RepeatModeEnum.FixedTime)
            {
                if (string.IsNullOrEmpty(jobInfo.FixedExpression))
                {
                    throw new Exception("Job按固定时间配置时，固定时间的表达式不能为空");
                }

                var expression = string.Empty;
                var fixedExs = jobInfo.FixedExpression.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var localParts = fixedExs[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                switch (jobInfo.FixedUnit)
                {
                    case FixedUnitEnum.Day:
                        var localMinute = Convert.ToInt32(localParts[1]);
                        var localSecond = Convert.ToInt32(localParts[2]);
                        var localHourList = new List<int>();
                        foreach (var item in fixedExs)
                        {
                            localParts = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            localHourList.Add(Convert.ToInt32(localParts[0]));
                        }
                        var localHours = string.Join(",", localHourList);
                        expression = string.Format("{0} {1} {2} * * ?", localSecond, localMinute, localHours);

                        break;

                    case FixedUnitEnum.Hour:
                        localSecond = Convert.ToInt32(localParts[1]);
                        var localMinuteList = new List<int>();
                        foreach (var item in fixedExs)
                        {
                            localParts = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            localMinuteList.Add(Convert.ToInt32(localParts[0]));
                        }
                        var localMinutes = string.Join(",", localMinuteList);
                        expression = string.Format("{0} {1} * * * ?", localSecond, localMinutes);

                        break;
                }

                builder = builder.WithCronSchedule(expression,
                    x => x.WithMisfireHandlingInstructionIgnoreMisfires().InTimeZone(TimeZoneInfo.Local));
            }

            if (jobInfo.StartTime < DateTime.Now.AddMinutes(1))
            {
                jobInfo.StartTime = DateTime.Now.AddMinutes(1);
            }
            builder.StartAt(jobInfo.StartTime);
            ITrigger trigger = builder.Build();

            HostScheduler.RegisterJob(innerJob, trigger);
        }

        public static void DeleteJob(JobInfo jobInfo)
        {
            if (string.IsNullOrEmpty(jobInfo.JobName) || string.IsNullOrEmpty(jobInfo.GroupName))
            {
                throw new Exception("job名称与组名都不能为空");
            }

            JobKey jobKey = new JobKey(jobInfo.JobName, jobInfo.GroupName);
            HostScheduler.RemoveJob(jobKey);
        }

        public static void DeleteJob(string jobName, string groupName)
        {
            if (string.IsNullOrEmpty(jobName) || string.IsNullOrEmpty(groupName))
            {
                throw new Exception("job名称与组名都不能为空");
            }

            JobKey jobKey = new JobKey(jobName, groupName);
            HostScheduler.RemoveJob(jobKey);
        }
    }
}
