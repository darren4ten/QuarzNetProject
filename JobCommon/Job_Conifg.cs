using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JobCommon.Model
{
    /// <summary>
    /// job配置
    /// </summary>
    [Serializable]
    public class Job_Config
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// job名称
        /// </summary>
        [Display(Name = "job名称")]
        public string JobName { get; set; }

        /// <summary>
        /// job显示名称
        /// </summary>
        [Display(Name = "job显示名称")]
        public string JobDisplayName { get; set; }

        /// <summary>
        /// 组名（GroupName和JobName可以确定job唯一性）
        /// </summary>
        [Display(Name = "组名")]
        public string GroupName { get; set; }

        /// <summary>
        /// 重复模式（1：一次  2：间隔时间  3：固定时间）
        /// </summary>
        [Display(Name = "Job重复模式")]
        public int RepeatMode { get; set; }

        /// <summary>
        /// 循环的单位
        /// </summary>
        [Display(Name = "循环的单位")]
        public int CycleUnit { get; set; }

        /// <summary>
        /// 循环间隔
        /// </summary>
        [Display(Name = "循环间隔")]
        public int Interval { get; set; }

        /// <summary>
        /// 程序集名称
        /// </summary>
        [Display(Name = "程序集名称")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        [Display(Name = "类名")]
        public string ClassName { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Display(Name = "是否可用")]
        public bool IsEnable { get; set; }

        /// <summary>
        /// job开始时间
        /// </summary>
        [Display(Name = "job开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "job创建时间")]
        public DateTime DataChange_CreateTime { get; set; }

        /// <summary>
        /// dll文件
        /// </summary>
        [Display(Name = "dll文件")]
        public string FilePath { get; set; }

        /// <summary>
        /// job配置更新后的状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 固定时间单位
        /// </summary>
        [Display(Name = "固定时间单位")]
        public int FixedUnit { get; set; }

        /// <summary>
        /// 固定时间的配置表达式 
        /// </summary>
        [Display(Name = "固定时间配置")]
        public string FixedExpression { get; set; }
    }

}
