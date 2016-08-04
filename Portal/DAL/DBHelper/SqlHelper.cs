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