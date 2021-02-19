using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LIMS.DC.Common.LOG;
using LIMS.DC.Model;
using LIMS.DC.DAHelper;
using LIMS.DC.DAL;

namespace LIMS.DC.Service
{
    public class DAManager: IPLCOperate
    {
        public DAManager()
        {
            OpcHelper.ServerShutDown += OpcService_ServerShutDown;
            OpcHelper.DataReceived += OpcService_DataReceived;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="dataID"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        /// <param name="stamp"></param>
        private void OpcService_DataReceived(int dataID, object value, string quality, DateTime stamp)
        {
            try
            {
                DCService.UpdateRealData(dataID, value, quality, stamp,DateTime.Now,1);
            }
            catch (Exception ex)
            {
                Log.WriteLog(E_ProcessLogType.Error, "更新实时数据失败。"+ex.Message, "OpcService_DataReceived()");
            }
        }
        /// <summary>
        /// opc服务器关闭
        /// </summary>
        /// <param name="reason"></param>
        private async void OpcService_ServerShutDown(string reason)
        {
            Log.WriteLog(E_ProcessLogType.Info, "opc服务器关闭。原因：" + reason, "OpcService_ServerShutDown()");

            /*
             由于下载opc组态会导致opc服务器关闭，根据要求需要在一段时候后进行重连。
             注意不要阻塞这个方法，因为西门子服务会回调这个方法，阻塞会导致OpcServer启动不起来。
             */
            await Task.Delay(TimeSpan.FromSeconds(20));
            await Task.Run(()=> {
                while (true)
                {
                    Disconnect();
                    if (Request())
                    {
                        Log.WriteLog(E_ProcessLogType.Info, "opc重连成功。", "OpcService_ServerShutDown()");
                        return;
                    }
                    Thread.Sleep(20000);
                }
            });
        }

        /// <summary>
        /// 数据访问类
        /// </summary>
        DC_Service DCService = new DC_Service();
        /// <summary>
        /// 日志
        /// </summary>
        SystemLog Log = new SystemLog("数据采集");
        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig()
        {
            foreach (var device in BaseData.Devices)
            {
                List<DC_DATA_CONFIG> lstConfig = BaseData.Configs.Where(s=>s.DEVICE_ID==device.ID).ToList();
                if (lstConfig != null && lstConfig.Count > 0)
                {
                    OpcHelper.OpcRegister(device, lstConfig);
                }
            }
        }
        /// <summary>
        /// 主入口
        /// </summary>
        /// <returns></returns>
        public bool Request()
        {
            try
            {
                OpcHelper.OpcConnect();//opc连接
            }
            catch (Exception ex)
            {
                Log.WriteLog(E_ProcessLogType.Error, "opc连接失败。" + ex.Message, "OpcService.OpcConnect()");
                return false;
            }
            if (!OpcHelper.IsConnected)
            {
                Log.WriteLog(E_ProcessLogType.Error, "opc连接失败。", "Request()");
                return false;
            }
            try
            {
                LoadConfig();//加载配置
            }
            catch (Exception ex)
            {
                Log.WriteLog(E_ProcessLogType.Error, "加载配置失败。" + ex.Message, "LoadConfig()");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                OpcHelper.OpcDisconnect();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 读
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public object Read(DC_DATA_CONFIG config)
        {
            return OpcHelper.Read(config);
        }
        /// <summary>
        /// 写
        /// </summary>
        /// <param name="config"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(DC_DATA_CONFIG config, object value)
        {
            return OpcHelper.Write(config, value);
        }
    }
}
