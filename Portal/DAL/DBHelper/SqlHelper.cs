using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Portal.DAL.DBHelper
{
    public class SqlHelper
    {
        public static readonly string ConnStr = @"Data Source=TENGDA\SQL2012;Initial Catalog=QuartzTest;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static SqlParameter[] FilterDbNullVals(params SqlParameter[] pars)
        {
            foreach (var item in pars)
            {
                if (item.Value == null)
                {
                    item.Value = DBNull.Value;
                }
            }

            return pars;
        }

        public static int GetValue(object val)
        {
            if (val is DBNull || val == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(val);
            }
        }

        public static string GetString(object val)
        {
            if (val == null || val is DBNull)
            {
                return null;
            }
            else
            {
                return val.ToString();
            }
        }

        /// <summary>
        /// 如果转换为null则返回Int32.MinValue
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int GetInt32(object val)
        {
            if (val is DBNull)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(val);
            }
        }

        public static double GetDouble(object val)
        {
            if (val is DBNull)
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(val);
            }
        }

        public static DateTime GetDateTime(object val)
        {
            if (val is DBNull)
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(val);
            }
        }

        public static DateTime? GetDateTimeNullable(object val)
        {
            if (val is DBNull || val == null)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(val);
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlHelper.FilterDbNullVals(pars);
                    cmd.Parameters.AddRange(pars);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlHelper.FilterDbNullVals(pars);
                    cmd.Parameters.AddRange(pars);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ExecuteDatatable(string sql, params SqlParameter[] pars)
        {
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(pars);
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        ad.Fill(ds);
                        return ds.Tables[0];
                    }
                }


            }
        }
    }
}