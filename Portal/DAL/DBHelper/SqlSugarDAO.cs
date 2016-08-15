using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.DAL.DBHelper
{
    public class SqlSugarDAO
    {
        private static SqlSugarClient _instance;
        //禁止实例化
        private SqlSugarDAO()
        {

        }
        public static SqlSugarClient GetInstance()
        {
            if (_instance == null)
            {
                string connection = System.Configuration.ConfigurationManager.ConnectionStrings[@"sqlConn"].ToString(); //这里可以动态根据cookies或session实现多库切换
                _instance = new SqlSugarClient(connection);
            }
            return _instance;

        }
    }
}