using JobCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobEntities
{
    public class demoCronJob : BaseJob
    {
        public override void Execute(string parameter)
        {
            Console.WriteLine(string.Format("demoCronJob:现在是{0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
        }
    }
}
