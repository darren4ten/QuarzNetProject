
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
        public int Delete(int id, bool isSoftDeleted = true)
        {
            string sql = "";
            if (isSoftDeleted)
            {
                sql = "update Job_Config set Status=3 where id=" + id;
            }
            else
            {
                sql = "delete from Job_Config where id=" + id;
            }
            return SqlHelper.ExecuteNonQuery(sql);
        }

        public int Add(Job_Config record)
        {
            string sql = @"insert into Job_Config output inserted.ID
                        values(@jname,@jdisplay,@gname,@cycleUnit,@interval,@assemblyName,@jclass,
                            @start,@enabled,@dtCreatTime,@dtLastTime,@filePath,@status,@repeatMode,@fixedUnit,@FixedExpression)";


            object autoId = SqlHelper.ExecuteScalar(sql,

                new SqlParameter("@jname", record.JobName),
                new SqlParameter("@gname", record.GroupName),
                new SqlParameter("@jdisplay", record.JobDisplayName),
                new SqlParameter("@jclass", record.ClassName),
                new SqlParameter("@assemblyName", record.AssemblyName),
                new SqlParameter("@filePath", record.FilePath),
                new SqlParameter("@repeatMode", Convert.ToInt32(record.RepeatMode)),
                new SqlParameter("@fixedUnit", record.FixedUnit),
                new SqlParameter("@FixedExpression", record.FixedExpression),
                new SqlParameter("@dtCreatTime", record.DataChange_CreateTime),
                new SqlParameter("@dtLastTime", ""),
                new SqlParameter("@cycleUnit", record.CycleUnit),
                new SqlParameter("@interval", record.Interval),
                new SqlParameter("@start", record.StartTime),
                new SqlParameter("@status", record.Status),
                new SqlParameter("@enabled", record.IsEnable)
                );
            return Convert.ToInt32(autoId);
        }

        public int Update(Job_Config record)
        {
            string sql = @"update Job_Config set JobName=@jname,GroupName=@gname,JobDisplayName=@jdisplay,ClassName=@jclass,
                    AssemblyName=@assemblyName,FilePath=@filePath,RepeatMode=@repeatMode,FixedUnit=@fixedUnit,FixedExpression=@FixedExpression,
                            DataChange_CreateTime=@dtCreatTime,CycleUnit=@cycleUnit,Interval=@interval,StartTime=@start,Status=@status,IsEnable=@enabled
                                    where ID=@id";

            return SqlHelper.ExecuteNonQuery(sql,
                new SqlParameter("@id", record.ID),
                new SqlParameter("@jname", record.JobName),
                new SqlParameter("@gname", record.GroupName),
                new SqlParameter("@jdisplay", record.JobDisplayName),
                new SqlParameter("@jclass", record.ClassName),
                new SqlParameter("@assemblyName", record.AssemblyName),
                new SqlParameter("@filePath", record.FilePath),
                new SqlParameter("@repeatMode", Convert.ToInt32(record.RepeatMode)),
                new SqlParameter("@fixedUnit", record.FixedUnit),
                new SqlParameter("@FixedExpression", record.FixedExpression),
                new SqlParameter("@dtCreatTime", record.DataChange_CreateTime),
                new SqlParameter("@cycleUnit", record.CycleUnit),
                new SqlParameter("@interval", record.Interval),
                new SqlParameter("@start", record.StartTime),
                new SqlParameter("@status", record.Status),
                new SqlParameter("@enabled", record.IsEnable)
                );
        }

        public Job_Config Get(int id)
        {
            string sql = "select top 1 * from Job_Config where id=@id";
            var dt = SqlHelper.ExecuteDatatable(sql, new SqlParameter("id", id));
            if (dt.Rows.Count > 0)
            {
                var r = GetJob_Config(dt.Rows[0]);
                return r;
            }
            else
            {
                return null;
            }

        }

        protected Job_Config GetJob_Config(DataRow row)
        {
            Job_Config r = new Job_Config();
            r.ID = SqlHelper.GetInt32(row["ID"].ToString());
            r.JobName = Convert.ToString(row["JobName"]);
            r.GroupName = Convert.ToString(row["GroupName"]);
            r.DataChange_CreateTime = Convert.ToDateTime(row["DataChange_CreateTime"]);
            r.JobDisplayName = Convert.ToString(row["JobDisplayName"]);
            r.ClassName = Convert.ToString(row["ClassName"]);
            r.FilePath = Convert.ToString(row["FilePath"]);
            r.RepeatMode = Convert.ToInt32(row["RepeatMode"]);
            r.FixedUnit = Convert.ToInt32(row["FixedUnit"]);
            r.FixedExpression = Convert.ToString(row["FixedExpression"]);
            r.CycleUnit = Convert.ToInt32(row["CycleUnit"]);
            r.Interval = Convert.ToInt32(row["Interval"]);
            r.StartTime = DateTime.Parse(Convert.ToString(row["StartTime"]));
            r.IsEnable = Convert.ToBoolean(row["IsEnable"]);
            return r;

        }


        public List<Job_Config> GetList(string sql, params SqlParameter[] pars)
        {
            var dt = SqlHelper.ExecuteDatatable(sql, pars);
            List<Job_Config> list = new List<Job_Config>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(GetJob_Config(item));
            }

            return list;
        }


    }
}