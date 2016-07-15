using JobCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEntities
{
    public class DemoJob : BaseJob
    {

        public override void Execute(string parameter)
        {
            Console.WriteLine("当前时间是：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}
