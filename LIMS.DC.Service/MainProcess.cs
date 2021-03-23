using LIMS.DC.Common.LOG;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    public class MainProcess
    {
        public static void Request()
        {
            DAManager daManager = new DAManager();

            //连接不成功，延迟10s后
            while ( !daManager.Request())
            {
                Thread.Sleep(10000);
            }

            TaskHelper.Init();


            //实时数据
            RealObjectManager realObjectManager = new RealObjectManager();
            realObjectManager.Request();


            //天车动作
            ActionManager actionManager = new ActionManager();
            actionManager.Operator = daManager;
            actionManager.Request();

            //校正时间
            CalManager calManager = new CalManager();
            calManager.Operator = daManager;
            calManager.Request();

            WriteObjectManager wManager = new WriteObjectManager();
            wManager.Operator = daManager;
            wManager.Request();
        }
    }
}
