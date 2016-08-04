using Common.Logging.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace JobAPI.Managers
{
    class HostSchedulerListener : ISchedulerListener
    {
        public void JobAdded(IJobDetail jobDetail)
        {

        }

        public void JobDeleted(JobKey jobKey)
        {

        }

        public void JobPaused(JobKey jobKey)
        {

        }

        public void JobResumed(JobKey jobKey)
        {

        }

        public void JobScheduled(ITrigger trigger)
        {

        }

        public void JobsPaused(string jobGroup)
        {

        }

        public void JobsResumed(string jobGroup)
        {

        }

        public void JobUnscheduled(TriggerKey triggerKey)
        {

        }

        public void SchedulerError(string msg, SchedulerException cause)
        {

        }

        public void SchedulerInStandbyMode()
        {

        }

        public void SchedulerShutdown()
        {

        }

        public void SchedulerShuttingdown()
        {

        }

        public void SchedulerStarted()
        {

        }

        public void SchedulerStarting()
        {
            // 检查trigger的NEXT_FIRE_TIME，如果小于当前时间，则将它的下次执行时间移动到一分钟后
            long currentTick = DateTime.UtcNow.AddMinutes(1).Ticks;


            NameValueCollection quartzs = ConfigurationManager.GetSection("quartz") as NameValueCollection;
            string connection = quartzs["quartz.dataSource.ds.connectionString"];

            // 1. 获取下次执行时间小于当前时间的trigger
            SqlConnection con = new SqlConnection(connection);
            string sql = "select SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP from QRTZ_TRIGGERS where NEXT_FIRE_TIME<" + currentTick;
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<HostTriggerModel> items = new List<HostTriggerModel>();
            while (reader.Read())
            {
                HostTriggerModel item = new HostTriggerModel();
                item.SCHED_NAME = reader["SCHED_NAME"].ToString();
                item.TRIGGER_GROUP = reader["TRIGGER_GROUP"].ToString();
                item.TRIGGER_NAME = reader["TRIGGER_NAME"].ToString();
                items.Add(item);
            }
            con.Close();

            if (items.Count == 0)
            {
                return;
            }

            // 2. 将下次执行时间设置为当前时间，让trigger只执行一次(否则trigger会重复调度job多次)
            string format = "update QRTZ_TRIGGERS set NEXT_FIRE_TIME={0}, TRIGGER_STATE='WAITING' where SCHED_NAME='{1}' and TRIGGER_GROUP='{2}' and TRIGGER_NAME='{3}'";
            con.Open();
            foreach (var item in items)
            {
                sql = string.Format(format, currentTick, item.SCHED_NAME, item.TRIGGER_GROUP, item.TRIGGER_NAME);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        public void SchedulingDataCleared()
        {

        }

        public void TriggerFinalized(ITrigger trigger)
        {

        }

        public void TriggerPaused(TriggerKey triggerKey)
        {

        }

        public void TriggerResumed(TriggerKey triggerKey)
        {

        }

        public void TriggersPaused(string triggerGroup)
        {

        }

        public void TriggersResumed(string triggerGroup)
        {

        }
    }
}
