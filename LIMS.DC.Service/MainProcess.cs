using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    public class MainProcess
    {
        public static void Request()
        {
            DAManager daManager = new DAManager();
            if( daManager.Request())
            {
                TaskHelper.Init();

                RealObjectManager realObjectManager = new RealObjectManager();
                realObjectManager.Request();

                ActionManager actionManager = new ActionManager();
                actionManager.Operator = daManager;
                actionManager.Request();

                CalManager calManager = new CalManager();
                calManager.Operator = daManager;
                calManager.Request();
            }
        }
    }
}
