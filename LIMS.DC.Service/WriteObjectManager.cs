using LIMS.DC.Common.LOG;
using LIMS.DC.DAL;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    public class WriteObjectManager
    {
        public List<WriteObject> WriteObjects { get; set; } = new List<WriteObject>();

        /// <summary>
        /// 数据访问对象
        /// </summary>
        DC_Service dC_Service = new DC_Service();

        /// <summary>
        /// 日志对象
        /// </summary>
        SystemLog log = new SystemLog("写入数据");

        /// <summary>
        /// plc操作对象
        /// </summary>
        public IPLCOperate Operator { get; set; }

        /// <summary>
        /// 入口
        /// </summary>
        public void Request()
        {
            try
            {
                LoadConfig();//加载配置
            }
            catch (Exception ex)
            {
                log.WriteLog(E_ProcessLogType.Error, "加载配置异常。" + ex.Message);
                return;
            }

            TaskHelper.RegisterTask(() =>
            {
                try
                {
                    List<DC_WRITE_DATA> datas = dC_Service.GetWriteDatasUpdated();
                    foreach (var obj in WriteObjects)
                    {
                        try
                        {
                            obj.Request(datas);
                        }
                        catch (Exception ex)
                        {
                            log.WriteLog(E_ProcessLogType.Error, "写入数据异常。" + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.WriteLog(E_ProcessLogType.Error, "查询已更新写入数据异常。" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadConfig()
        {
            List<DC_WRITE_DATA> datas = dC_Service.GetWriteDatas();
            foreach (var item in datas)
            {
                WriteObject writeObject = new WriteObject();
                writeObject.Config = BaseData.Configs.FirstOrDefault(s => s.ID == item.DATA_CONFIG_ID);
                writeObject.Operator = Operator;
                WriteObjects.Add(writeObject);
            }
        }
    }
}
