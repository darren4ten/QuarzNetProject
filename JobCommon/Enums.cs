using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobCommon.Model
{
    /// <summary>
    /// JOB重复的模式
    /// </summary>
    public enum RepeatModeEnum
    {
        /// <summary>
        /// 一次性
        /// </summary>
        OneTime = 1,

        /// <summary>
        /// 循环
        /// </summary>
        Interval = 2,

        /// <summary>
        /// 固定时间
        /// </summary>
        FixedTime = 3
    }

    /// <summary>
    /// JOB循环单位
    /// </summary>
    public enum CycleUnitEnum
    {
        /// <summary>
        /// 按天循环
        /// </summary>
        Day = 0,

        /// <summary>
        /// 按小时循环
        /// </summary>
        Hour = 1,

        /// <summary>
        /// 按分钟循环
        /// </summary>
        Minute = 2,

        /// <summary>
        /// 按秒循环
        /// </summary>
        Second = 3
    }

    /// <summary>
    /// 固定时间单位
    /// </summary>
    public enum FixedUnitEnum
    {
        Second = 0,
        Min = 1,
        Day = 2,
        Hour = 3,
        Custom = 4//自定义
    }

    /// <summary>
    /// JOB更新状态
    /// </summary>
    public enum JobStatusEnum
    {
        /// <summary>
        /// 无修改
        /// </summary>
        NoChange = 0,

        /// <summary>
        /// 有更新
        /// </summary>
        Updated = 1,

        /// <summary>
        /// 新增job
        /// </summary>
        Add = 2,

        /// <summary>
        /// 已删除
        /// </summary>
        Delete = 3
    }
}
