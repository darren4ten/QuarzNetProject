using Common.Logging;
using JobCommon;
using Quartz;
using Quartz.Impl;
using JobAPI.Managers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JobCommon.Model;

namespace JobAPI
{
    public class HostJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string className = context.JobDetail.JobDataMap.Get(Consts.JobClassName).ToString();
            string assemblyPath = context.JobDetail.JobDataMap.Get(Consts.JobAssemblyPath).ToString();
            try
            {
                Assembly assembly = Consts.GetAssembly(assemblyPath);
                BaseJob job = assembly.CreateInstance(className) as BaseJob;

                object p = context.JobDetail.JobDataMap.Get(Consts.JobParameter);
                string ps = p == null ? string.Empty : p.ToString();

                job.Execute(ps);

                WriteJobLog((JobDetailImpl)context.JobDetail);
            }
            catch (Exception ex)
            {
                Exception wrapper = new Exception(string.Format("Job异常, assembly:{0}; class:{1}", assemblyPath, className), ex);
                // LogManager.LogException(wrapper);

                WriteJobLog((JobDetailImpl)context.JobDetail, ex);
            }
        }

        /// <summary>
        /// 写job运行日志
        /// </summary>
        private void WriteJobLog(JobDetailImpl jobDetail, Exception ex = null)
        {
            string jobName = jobDetail.Name;
            string groupName = jobDetail.Group;

            string sql = string.Empty;
            if (ex != null)
            {
                var message = ex.Message.Replace('\'', ' ');
                sql = string.Format("INSERT INTO Job_Log(GroupName,JobName,Result,Message,CreateTime) VALUES('{0}','{1}','{2}','{3}','{4}')", groupName, jobName, "F", ex.Message, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            else
            {
                sql = string.Format("INSERT INTO Job_Log(GroupName,JobName,Result,CreateTime) VALUES('{0}','{1}','{2}','{3}')", groupName, jobName, "T", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            ExecuteSql(sql);
        }

        private void ExecuteSql(string sql)
        {
            try
            {
                SqlConnection con = new SqlConnection(Consts.ConnectionString);
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                //LogManager.LogException(ex);
            }

        }
    }
}
