using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEntities
{

    /// <summary>
    /// job基类，自定义job需继承该类
    /// </summary>
    public abstract class BaseJob1
    {
        /// <summary>
        /// job的业务逻辑
        /// </summary>
        public abstract void Execute(string parameter);
    }

}
