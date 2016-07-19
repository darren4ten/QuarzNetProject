
using JobCommon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class JobRecord
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Job名称")]
        public string JobName { get; set; }
        [Display(Name = "组名")]
        public string GroupName { get; set; }

        [Display(Name = "Job显示名称")]
        public string JobDisplayName { get; set; }

        [Display(Name = "Job描述")]
        public string JobDescription { get; set; }
        [Display(Name = "Job完整类名")]
        public string JobClassName { get; set; }
        [Display(Name = "DLL文件路径")]
        public string AssemblyPath { get; set; }

        [Display(Name = "重复模式")]
        public RepeatModeEnum RepeatMode { get; set; }

        [Display(Name = "固定时间单位")]
        public FixedUnitEnum FixedUnit { get; set; }

        [Display(Name = "固定时间表达式")]
        public string FixedExpression { get; set; }

        [Display(Name = "循环单位")]
        public CycleUnitEnum CycleUnit { get; set; }

        [Display(Name = "循环间隔")]
        public int Interval { get; set; }

        /// <summary>
        /// job开始时间
        /// </summary>
        [Display(Name = "Job开始时间")]
        public DateTime StartTime { get; set; }

        [Display(Name = "是否可用")]
        public bool IsEnabled { get; set; }
    }
}