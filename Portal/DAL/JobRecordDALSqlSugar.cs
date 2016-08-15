
using JobCommon.Model;
using Portal.DAL.DBHelper;
using Portal.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace Portal.DAL
{
    public class JobRecordDALSqlSugar
    {
        private SqlSugarClient _sqlClient = SqlSugarDAO.GetInstance();
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
            return _sqlClient.ExecuteCommand(sql);

        }

        public int Add(Job_Config record)
        {

            object newRec = _sqlClient.Insert(record);
            if (newRec != null)
            {
                return Convert.ToInt32(((Job_Config)newRec).ID);
            }
            return -1;
        }

        public int Update(Job_Config record)
        {
            return _sqlClient.Update<Job_Config>(record, p => p.ID == record.ID) ? 1 : -1;
        }

        public Job_Config Get(int id)
        {

            return _sqlClient.Queryable<Job_Config>().Where(p => p.ID == id).FirstOrDefault();
        }


        public List<Job_Config> GetList(string sql, params SqlParameter[] pars)
        {
            return _sqlClient.GetList<Job_Config>(sql, pars);
        }


    }
}