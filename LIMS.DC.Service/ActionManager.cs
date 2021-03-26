using LIMS.DC.Common.LOG;
using LIMS.DC.DAL;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    class ActionManager
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        SystemLog log = new SystemLog("动作数据");

        /// <summary>
        /// plc操作对象
        /// </summary>
        public IPLCOperate Operator { get; set; }

        List<ActionObject> ActionObjects = new List<ActionObject>();

        public void Request()
        {
            try
            {
                LoadConfig();
            }
            catch (Exception ex)
            {
                log.WriteLog(E_ProcessLogType.Error, $"加载动作配置异常。" + ex.Message);
                return;
            }

            //读取动作
            TaskHelper.RegisterTask(() =>
            {
                foreach (var obj in ActionObjects)
                {
                    try
                    {
                        obj.Request();
                    }
                    catch (Exception ex)
                    {
                        log.WriteLog(E_ProcessLogType.Error, $"读取动作异常。cra:{obj.Cra_ID}。" + ex.Message);
                    }
                }
            });
        }

        private void LoadConfig()
        {
            List<DC_DEVICE> devices = BaseData.Devices.Where(s => s.CRA_ID != null).ToList();
            foreach (var device in devices)
            {
                LoadConfig(device);
            }
        }

        private void LoadConfig(DC_DEVICE device )
        {
            DC_DATA_CONFIG time = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_TIME");
            DC_DATA_CONFIG x = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_X");
            DC_DATA_CONFIG y = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_Y");
            DC_DATA_CONFIG l_y = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_LITTLE_Y");
            DC_DATA_CONFIG z = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_Z");
            DC_DATA_CONFIG l_z = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_LITTLE_Z");
            DC_DATA_CONFIG wgt = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_WEIGHT");
            DC_DATA_CONFIG l_wgt = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_LITTLE_WEIGHT");
            DC_DATA_CONFIG symbol = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_SYMBOL");
            DC_DATA_CONFIG count = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_OBJ_COUNT");
            DC_DATA_CONFIG cNum = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_CACHE_NUM");
            DC_DATA_CONFIG rNum = BaseData.Configs.FirstOrDefault(s => s.DEVICE_ID == device.ID && s.NUM == "ACTION_READED_NUM");
            ActionObject actionObject = new ActionObject();
            actionObject.Operator = Operator;
            actionObject.Cra_ID = (int)device.CRA_ID;
            actionObject.ACTION_TIME = time;
            actionObject.ACTION_X = x;
            actionObject.ACTION_Y = y;
            actionObject.ACTION_LITTLE_Y = l_y;
            actionObject.ACTION_Z = z;
            actionObject.ACTION_LITTLE_Z = l_z;
            actionObject.ACTION_WEIGHT = wgt;
            actionObject.ACTION_LITTLE_WEIGHT = l_wgt;
            actionObject.ACTION_SYMBOL = symbol;
            actionObject.ACTION_OBJ_COUNT = count;
            actionObject.ACTION_CACHE_NUM = cNum;
            actionObject.ACTION_READED_NUM = rNum;
            ActionObjects.Add(actionObject);
        }
    }
}
