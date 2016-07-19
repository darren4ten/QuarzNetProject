using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JobCommon.Model
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Consts
    {
        /// <summary>
        /// JOB参数
        /// </summary>
        public static string JobParameter = "parameter";

        /// <summary>
        /// JOB程序集
        /// </summary>
        public static string JobAssembly = "assembly";

        /// <summary>
        /// JOB程序集路径
        /// </summary>
        public static string JobAssemblyPath = "assemblyPath";

        /// <summary>
        /// JOB类名
        /// </summary>
        public static string JobClassName = "classname";

        private static string connectionString;
        /// <summary>
        /// 连接串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    return @"Data Source=TENGDA\SQL2012;Initial Catalog=QuartzTest;Integrated Security=True;Connect Timeout=35;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                }
                return connectionString;
            }
        }

        /// <summary>
        /// JOB所在的程序集
        /// </summary>
        private static Dictionary<string, Assembly> Assemblies = new Dictionary<string, Assembly>();

        /// <summary>
        /// 根据路径获取程序集
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string filepath)
        {
            if (!Assemblies.ContainsKey(filepath))
            {
                Assembly assembly = Assembly.LoadFile(filepath);
                Assemblies.Add(filepath, assembly);
            }
            return Assemblies[filepath];
        }
    }
}
