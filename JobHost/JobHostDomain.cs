using JobAPI.Managers;
using JobCommon.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobHost
{
    public class JobHostDomain
    {
        private AppDomain jobDomain;
        private string runPath = ConfigurationManager.AppSettings["Folder_Job_Run"];
        private string configPath = ConfigurationManager.AppSettings["Folder_Job_Config"];
        private readonly ILog logger = LogManager.GetLogger(typeof(JobHostDomain));

        public void Run()
        {
            // 检查job所在的dll目录是否存在
            if (!Directory.Exists(runPath))
            {
                Directory.CreateDirectory(runPath);
            }

            jobDomain = CreateJobDomain();
            jobDomain.DoCallBack(SchedulerManager.Start);

            // 同步job配置
            Task.Factory.StartNew(() => { SyncJobConfigHandler(); });

            //// 删除超过7天的job日志
            //Task.Factory.StartNew(() => { DeleteLogsHandler(); });
        }

        // 停止
        public void Stop()
        {
            if (jobDomain != null)
            {
                jobDomain.DoCallBack(SchedulerManager.Stop);
            }
        }

        /// <summary>
        /// 同步JOB配置handler
        /// </summary>
        private void SyncJobConfigHandler()
        {
            while (true)
            {
                try
                {
                    SyncJobConfig();
                }
                catch (Exception ex)
                {
                    var wrapper = new Exception("同步job配置失败", ex);
                }
                Thread.Sleep(60 * 1000);            // 每分钟循环一次
            }
        }

        /// <summary>
        /// 同步JOB配置
        /// </summary>
        private void SyncJobConfig()
        {
            // 1. 获取有更新的配置
            var list = GetJobConfig();
            if (list.Count == 0)
            {
                return;
            }

            // 2. 停止无效job的运行
            StopInvalidJob(list);

            // 如果不存在新增的job，或修改后IsEnable为true的job，则不需要执行后续操作
            list = list.Where(x => x.Status == (int)JobStatusEnum.Add || (x.Status == (int)JobStatusEnum.Updated && x.IsEnable)).ToList();
            if (list.Count == 0)
            {
                return;
            }

            // 3. 停止job scheduler
            jobDomain.DoCallBack(SchedulerManager.Stop);

            // 4. 卸载job appdomain
            AppDomain.Unload(jobDomain);

            // 5. 覆盖新版dll
            OverrideDll(list);

            // 6. 重新加载job appdomain
            jobDomain = CreateJobDomain();

            // 7. 启动scheduler
            jobDomain.DoCallBack(SchedulerManager.Start);

            // 8. 同步job
            SyncJob(list);
        }

        /// <summary>
        /// 获取有更新的job配置
        /// </summary>
        private List<Job_Config> GetJobConfig()
        {
            var cons = AppConfigHelper.GetQuartzConnString();
            var con = new SqlConnection(cons);
            var sql = string.Format("select * from Job_Config where status!={0}", (int)JobStatusEnum.NoChange);
            var cmd = new SqlCommand(sql, con);
            var list = new List<Job_Config>();
            using (con)
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var item = new Job_Config();
                    item.ID = Convert.ToInt64(reader["ID"]);
                    item.JobName = reader["JobName"].ToString();
                    item.FilePath = reader["FilePath"].ToString();
                    item.CycleUnit = Convert.ToInt32(reader["CycleUnit"]);
                    item.Interval = Convert.ToInt32(reader["Interval"]);
                    item.ClassName = reader["ClassName"].ToString();
                    item.StartTime = Convert.ToDateTime(reader["StartTime"]);
                    item.IsEnable = Convert.ToBoolean(reader["IsEnable"]);
                    item.FilePath = reader["FilePath"].ToString();
                    item.Status = Convert.ToInt32(reader["Status"]);
                    item.GroupName = reader["GroupName"].ToString();
                    item.RepeatMode = Convert.ToInt32(reader["RepeatMode"]);
                    item.FixedUnit = Convert.ToInt32(reader["FixedUnit"]);
                    item.FixedExpression = reader["FixedExpression"].ToString();

                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// 停止无效job的运行（通过删除job实现）
        /// </summary>
        private void StopInvalidJob(List<Job_Config> list)
        {
            var invalidList = list.Where(x => x.Status == (int)JobStatusEnum.Updated || x.Status == (int)JobStatusEnum.Delete).ToList();
            if (invalidList.Count == 0)
            {
                return;
            }

            var wrapper = new CallWrapper();
            foreach (var item in invalidList)
            {
                var jobInfo = new JobInfo { GroupName = item.GroupName, JobName = item.JobName };
                wrapper.JobInfo = jobInfo;
                wrapper.CallDeleteJob();

                if (item.Status == (int)JobStatusEnum.Updated && !item.IsEnable)
                {
                    UpdateJobConfig(item.ID);
                }

                if (item.Status == (int)JobStatusEnum.Delete)
                {
                    DeleteJobConfig(item.ID);
                }
            }
        }

        /// <summary>
        /// 将job配置转换成job参数
        /// </summary>
        private JobInfo GetJobInfo(Job_Config config)
        {
            var jobInfo = new JobInfo
            {
                GroupName = config.GroupName,
                ClassName = config.ClassName,
                CanConcurrent = false,
                CycleUnit = (CycleUnitEnum)config.CycleUnit,
                Interval = config.Interval,
                JobName = config.JobName,
                RepeatMode = (RepeatModeEnum)config.RepeatMode,
                StartTime = config.StartTime,
                FileName = config.FilePath,
                FixedUnit = (FixedUnitEnum)config.FixedUnit,
                FixedExpression = config.FixedExpression
            };
            return jobInfo;
        }

        /// <summary>
        /// 创建job运行appdomain
        /// </summary>
        /// <returns></returns>
        private AppDomain CreateJobDomain()
        {
            var domainSetup = AppDomain.CurrentDomain.SetupInformation;
            var jobDomain = AppDomain.CreateDomain("jobdomain", AppDomain.CurrentDomain.Evidence, domainSetup);
            return jobDomain;
        }

        /// <summary>
        /// job配置完成同步，将job配置设置为无修改状态
        /// </summary>
        private void UpdateJobConfig(long jobID)
        {
            var sql = string.Format("update Job_Config set status = {0} where ID={1}", (int)JobStatusEnum.NoChange, jobID);
            var cons = AppConfigHelper.GetQuartzConnString();
            using (var con = new SqlConnection(cons))
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// job配置完成同步，删除
        /// </summary>
        /// <param name="jobID"></param>
        private void DeleteJobConfig(long jobID)
        {
            var sql = string.Format("delete from Job_Config where ID={0}", jobID);
            var cons = AppConfigHelper.GetQuartzConnString();

            using (var con = new SqlConnection(cons))
            {
                con.Open();
                var cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 用新版dll覆盖
        /// </summary>
        private void OverrideDll(List<Job_Config> list)
        {
            var spath = string.Empty;
            var tpath = string.Empty;
            var files = new List<KeyValuePair<string, string>>();
            foreach (var item in list)
            {
                spath = string.Format(@"{0}\{1}", configPath, item.FilePath.Substring(item.FilePath.LastIndexOf('\\') + 1));
                tpath = string.Format(@"{0}\{1}", runPath, item.FilePath.Substring(item.FilePath.LastIndexOf('\\') + 1));
                if (!files.Exists(x => x.Key == spath))
                {
                    files.Add(new KeyValuePair<string, string>(spath, tpath));
                }
                item.FilePath = tpath;
            }

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    File.Copy(file.Key, file.Value, true);      // 覆盖dll
                }
            }
        }

        /// <summary>
        /// 同步配置
        /// </summary>
        /// <param name="list"></param>
        private void SyncJob(List<Job_Config> list)
        {
            var wrapper = new CallWrapper();

            foreach (var item in list)
            {
                try
                {
                    wrapper.JobInfo = GetJobInfo(item);
                    jobDomain.DoCallBack(new CrossAppDomainDelegate(wrapper.CallAddJob));
                    UpdateJobConfig(item.ID);
                }
                catch (Exception ex)
                {
                    var outerEx = new Exception(string.Format("同步Job失败。JobName：{0}", item.JobName), ex);
                    logger.Error(outerEx);
                }
            }
        }



        //-------------------------------------------------------------------------------------------------//
        //-------------------------------------------------------------------------------------------------//
        //-------------------------------------------------------------------------------------------------//



        /// <summary>
        /// 删除超过7天的日志Handler
        /// </summary>
        private void DeleteLogsHandler()
        {
            while (true)
            {
                try
                {
                    DeleteLogs();
                }
                catch (Exception ex)
                {
                    var wrapper = new Exception("删除超过7天日志失败", ex);
                    logger.Error(wrapper);
                }
                Thread.Sleep(5 * 60 * 1000);        // 每5分钟执行一次
            }
        }

        /// <summary>
        /// 删除超过7天的日志
        /// </summary>
        private void DeleteLogs()
        {
            string sql = string.Format("delete from Job_Log where ExecuteTime<'{0}'", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));
            var cons = AppConfigHelper.GetQuartzConnString();
            using (SqlConnection con = new SqlConnection(cons))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }

        }

    }

    [Serializable]
    public class CallWrapper
    {
        private JobManager jobManager = new JobManager();
        public JobInfo JobInfo { get; set; }

        /// <summary>
        /// 添加job操作（跨域调用）
        /// </summary>
        public void CallAddJob()
        {
            jobManager.AddJob(JobInfo);
        }

        /// <summary>
        /// 删除job操作（跨域调用）
        /// </summary>
        public void CallDeleteJob()
        {
            jobManager.DeleteJob(JobInfo);
        }
    }
}
