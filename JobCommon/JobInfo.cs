using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCommon.Model
{
    public class JobInfo
    {
        /// <summary>
        /// 定义job的类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// job所在的程序集
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// job组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// job开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 重复模式
        /// </summary>
        public RepeatModeEnum RepeatMode { get; set; }

        /// <summary>
        /// 循环单位
        /// </summary>
        public CycleUnitEnum CycleUnit { get; set; }

        /// <summary>
        /// 循环间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 是否可以多线程并行执行
        /// </summary>
        public bool CanConcurrent { get; set; }

        /// <summary>
        /// dll文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 固定时间单位
        /// </summary>
        public FixedUnitEnum FixedUnit { get; set; }

        /// <summary>
        /// 固定时间的配置表达式
        /// </summary>
        public string FixedExpression { get; set; }
    }
}
