using LIMS.DC.Common.LOG;
using LIMS.DC.Model;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    class CalManager
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        SystemLog log = new SystemLog("校正时间");

        /// <summary>
        /// plc操作对象
        /// </summary>
        public IPLCOperate Operator { get; set; }

        List<CalObject> CalObjects = new List<CalObject>();

        static IScheduler scheduler;

        public async void Request()
        {
            try
            {
                LoadConfig();
            }
            catch (Exception ex)
            {
                log.WriteLog(E_ProcessLogType.Error, "加载校正时间配置异常。" + ex.Message);
                return;
            }

            StdSchedulerFactory facotry = new StdSchedulerFactory();
            scheduler = await facotry.GetScheduler();
            await scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<Job_Cal_Time>().Build();
            jobDetail.JobDataMap.Add("para", CalObjects);
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule("0 0 2 * * ?").StartNow().Build();//每天凌晨2点执行
            await scheduler.ScheduleJob(jobDetail, trigger);
        }

        private void LoadConfig()
        {
            List<DC_DEVICE> devices = BaseData.Devices.Where(s => s.CRA_ID != null).ToList();
            foreach (var device in devices)
            {
                LoadConfig(device);
            }
        }

        private void LoadConfig(DC_DEVICE device)
        {
            DC_DATA_CONFIG time = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "CAL_TIME");
            DC_DATA_CONFIG flag = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "CAL_WRITE_FLAG");
            CalObject calObject = new CalObject();
            calObject.Operator = Operator;
            calObject.CAL_TIME = time;
            calObject.CAL_WRITE_FLAG = flag;
            CalObjects.Add(calObject);
        }
    }
}
