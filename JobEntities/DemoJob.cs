using JobCommon;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEntities
{
    public class DemoJob : BaseJob
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(DemoJob));
        public override void Execute(string parameter)
        {
            Console.WriteLine("当前时间是：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            logger.Info(string.Format("参数{0},当前时间是{1}：", parameter, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
        }
    }
}
