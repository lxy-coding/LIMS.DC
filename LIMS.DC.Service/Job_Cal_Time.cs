using LIMS.DC.Common.LOG;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    [DisallowConcurrentExecution]
    class Job_Cal_Time : IJob
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        SystemLog log = new SystemLog("校正时间");

        public async Task Execute(IJobExecutionContext context)
        {
            List<CalObject> CalObjects = (List<CalObject>)context.JobDetail.JobDataMap["para"];
            await Task.Run(() =>
            {
                foreach (var obj in CalObjects)
                {
                    try
                    {
                        bool res= obj.Request();
                        if(res)
                        {
                            log.WriteLog(E_ProcessLogType.Error, $"校正时间成功。DEVICE_ID={obj.CAL_TIME.DEVICE_ID}");
                        }
                        else
                        {
                            log.WriteLog(E_ProcessLogType.Error, $"校正时间失败。DEVICE_ID={obj.CAL_TIME.DEVICE_ID}");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteLog(E_ProcessLogType.Error, $"校正时间异常。DEVICE_ID={obj.CAL_TIME.DEVICE_ID}" + ex.Message);
                    }
                }
            });
        }
    }
}
