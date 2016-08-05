using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobAPI
{
    public class AppConfigHelper
    {
        /// <summary>
        /// 获取Quartz配置的连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetQuartzConnString()
        {
            return @"Data Source=TENGDA\SQL2012;Initial Catalog=QuartzTest;Integrated Security=True;Connect Timeout=35;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
    }
}
