using JobCommon.Model;
using Portal.DAL.DBHelper;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Portal.DAL
{
    public class JobRecordDAL
    {
        public int Add(JobRecord record)
        {
            string sql = "insert into JobRecord values(@id,@jname,@gname,@jdisplay,@jdesc,@jclass,@assembly,@repeatMode,@fixedUnit,@FixeExpression,@cycleUnit,@interval,@start,@enabled)";


            return SqlHelper.ExecuteNonQuery(sql,
                new SqlParameter("@id", record.ID),
                new SqlParameter("@jname", record.JobName),
                new SqlParameter("@gname", record.GroupName),
                new SqlParameter("@jdisplay", record.JobDisplayName),
                new SqlParameter("@jdesc", record.JobDescription),
                new SqlParameter("@jclass", record.JobClassName),
                new SqlParameter("@assembly", record.AssemblyPath),
                new SqlParameter("@repeatMode", Convert.ToInt32(record.RepeatMode)),
                new SqlParameter("@fixedUnit", record.FixedUnit),
                new SqlParameter("@FixeExpression", record.FixedExpression),
                new SqlParameter("@cycleUnit", record.CycleUnit),
                new SqlParameter("@interval", record.Interval),
                new SqlParameter("@start", record.StartTime),
                 new SqlParameter("@enabled", record.IsEnabled)
                );
        }

        public int Update(JobRecord record)
        {
            string sql = @"update JobRecord set JobName=@jname,GroupName=@gname,
                           JobDisplayName=@jdisplay,JobDescription=@jdesc,JobClassName=@jclass,AssemblyPath=@assembly,RepeatMode=@repeatMode,
                                    FixedUnit=@fixedUnit,FixedExpression=@FixeExpression,
                                CycleUnit=@cycleUnit,Interval=@interval,StartTime=@start,IsEnabled=@enabled
                                    where ID=@id";


            return SqlHelper.ExecuteNonQuery(sql,
                new SqlParameter("@id", record.ID),
                new SqlParameter("@jname", record.JobName),
                new SqlParameter("@gname", record.GroupName),
                new SqlParameter("@jdisplay", record.JobDisplayName),
                new SqlParameter("@jdesc", record.JobDescription),
                new SqlParameter("@jclass", record.JobClassName),
                new SqlParameter("@assembly", record.AssemblyPath),
                new SqlParameter("@repeatMode", Convert.ToInt32(record.RepeatMode)),
                new SqlParameter("@fixedUnit", record.FixedUnit),
                new SqlParameter("@FixeExpression", record.FixedExpression),
                new SqlParameter("@cycleUnit", record.CycleUnit),
                new SqlParameter("@interval", record.Interval),
                new SqlParameter("@start", record.StartTime),
                 new SqlParameter("@enabled", record.IsEnabled)
                );
        }

        public JobRecord Get(Guid id)
        {
            string sql = "select top 1 * from JobRecord where id=@id";
            var dt = SqlHelper.ExecuteDatatable(sql, new SqlParameter("id", id.ToString()));
            if (dt.Rows.Count > 0)
            {
                //var row = dt.Rows[0];
                //JobRecord r = new JobRecord();
                //r.ID = id;
                //r.JobName = Convert.ToString(row["JobName"]);
                //r.GroupName = Convert.ToString(row["GroupName"]);
                //r.JobDescription = Convert.ToString(row["JobDescription"]);
                //r.JobDisplayName = Convert.ToString(row["JobDisplayName"]);
                //r.JobClassName = Convert.ToString(row["JobClassName"]);
                //r.AssemblyPath = Convert.ToString(row["AssemblyPath"]);
                //r.RepeatMode = (RepeatModeEnum)Enum.Parse(typeof(RepeatModeEnum), Convert.ToString(row["RepeatMode"]));
                //r.FixedUnit = (FixedUnitEnum)Enum.Parse(typeof(FixedUnitEnum), Convert.ToString(row["FixedUnit"]));
                //r.FixedExpression = Convert.ToString(row["FixedExpression"]);
                //r.CycleUnit = (CycleUnitEnum)Enum.Parse(typeof(CycleUnitEnum), Convert.ToString(row["CycleUnit"]));
                //r.Interval = SqlHelper.GetValue(row["Interval"]);
                //r.StartTime = DateTime.Parse(Convert.ToString(row["StartTime"]));
                //r.IsEnabled = Convert.ToBoolean(row["IsEnabled"]);
                var r = GetRecord(dt.Rows[0]);
                return r;
            }
            else
            {
                return null;
            }

        }

        protected JobRecord GetRecord(DataRow row)
        {
            JobRecord r = new JobRecord();
            r.ID = Guid.Parse(row["ID"].ToString());
            r.JobName = Convert.ToString(row["JobName"]);
            r.GroupName = Convert.ToString(row["GroupName"]);
            r.JobDescription = Convert.ToString(row["JobDescription"]);
            r.JobDisplayName = Convert.ToString(row["JobDisplayName"]);
            r.JobClassName = Convert.ToString(row["JobClassName"]);
            r.AssemblyPath = Convert.ToString(row["AssemblyPath"]);
            r.RepeatMode = (RepeatModeEnum)Enum.Parse(typeof(RepeatModeEnum), Convert.ToString(row["RepeatMode"]));
            r.FixedUnit = (FixedUnitEnum)Enum.Parse(typeof(FixedUnitEnum), Convert.ToString(row["FixedUnit"]));
            r.FixedExpression = Convert.ToString(row["FixedExpression"]);
            r.CycleUnit = (CycleUnitEnum)Enum.Parse(typeof(CycleUnitEnum), Convert.ToString(row["CycleUnit"]));
            r.Interval = SqlHelper.GetValue(row["Interval"]);
            r.StartTime = DateTime.Parse(Convert.ToString(row["StartTime"]));
            r.IsEnabled = Convert.ToBoolean(row["IsEnabled"]);
            return r;

        }


        public List<JobRecord> GetList(string sql, params SqlParameter[] pars)
        {
            var dt = SqlHelper.ExecuteDatatable(sql, pars);
            List<JobRecord> list = new List<JobRecord>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(GetRecord(item));
            }

            return list;
        }


    }
}