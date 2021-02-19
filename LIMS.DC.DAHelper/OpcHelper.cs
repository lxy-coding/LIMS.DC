using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LIMS.DC.Model;
namespace LIMS.DC.DAHelper
{
    public class OpcHelper
    {
        static OpcHelper()
        {
            HostName = Dns.GetHostName();//获取主机名
            OpcServer.Instance.ServerShutDown += OpcService_ServerShutDown;
        }
        /// <summary>
        /// OpcServer停止工作事件
        /// </summary>
        public static event ServerShutDownEventHandler ServerShutDown;
        /// <summary>
        /// 服务器停止工作
        /// </summary>
        /// <param name="reason"></param>
        private static void OpcService_ServerShutDown(string reason)
        {
            ServerShutDown?.Invoke(reason);
        }
        /// <summary>
        /// opc名称
        /// </summary>
        private const string OpcName = "OPC.SimaticNet";
        /// <summary>
        /// 主机名
        /// </summary>
        private static string HostName;
        /// <summary>
        /// opc连接
        /// </summary>
        /// <returns></returns>
        public static bool OpcConnect()
        {
            return OpcServer.Instance.OpcConnect(OpcName, HostName);
        }
        /// <summary>
        /// opc释放连接
        /// </summary>
        public static void OpcDisconnect()
        {
            OpcServer.Instance.OpcDisConnect();
        }
        /// <summary>
        /// opc注册组和数据点
        /// </summary>
        /// <param name="device"></param>
        /// <param name="configs"></param>
        public static void OpcRegister(DC_DEVICE device, List<DC_DATA_CONFIG> configs)
        {
            OpcGroup opcGroup = new OpcGroup(device.ID);//创建组对象
            OpcServer.Instance.AddGroup(opcGroup);//组添加到server
            opcGroup.DataReceived += OpcGroup_DataReceived;
            opcGroup.IsSubscribed = true;
            foreach (var config in configs)
            {
                OpcItem opcItem = new OpcItem(config.ID, device.CHANNEL + ":[" + device.S7CONNECTION + "]" + config.MEMORY_ADDRESS);//创建数据项对象
                opcItem.IsActive = config.SUBSCRIPTION==1?1:0;//当为0时，可读写，但不会订阅数据;为1时，可读写,会订阅数据
                opcGroup.AddItem(opcItem);//项添加到组
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataID"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        /// <param name="stamp"></param>
        private static void OpcGroup_DataReceived(int dataID, object value, string quality, DateTime stamp)
        {
            DataReceived?.Invoke(dataID, value, quality, stamp);
        }
        /// <summary>
        /// 接收数据事件
        /// </summary>
        public static event DataReceivedEventHandler DataReceived;
        /// <summary>
        /// 是否连接
        /// </summary>
        public static bool IsConnected { get { return OpcServer.Instance.Connected; } }
        /// <summary>
        /// 同步读
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <returns></returns>
        public static object Read(DC_DATA_CONFIG config)
        {
            if(config.DEVICE_ID==null)
            {
                throw new NullReferenceException("DEVICE_ID为空，无法进行读写");
            }
            int id =(int) config.DEVICE_ID;
            return OpcServer.Instance.OpcGroups[id].Read(config.ID);
        }
        /// <summary>
        /// 同步写
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <returns></returns>
        public static bool Write(DC_DATA_CONFIG config, object value)
        {
            if (config.DEVICE_ID == null)
            {
                throw new NullReferenceException("DEVICE_ID为空，无法进行读写");
            }
            int id = (int)config.DEVICE_ID;
            return OpcServer.Instance.OpcGroups[id].Write(config.ID, value);
        }
        /// <summary>
        /// 异步读
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <returns></returns>
        public static void ReadAsync(DC_DATA_CONFIG config)
        {
            if (config.DEVICE_ID == null)
            {
                throw new NullReferenceException("DEVICE_ID为空，无法进行读写");
            }
            int id = (int)config.DEVICE_ID;
            OpcServer.Instance.OpcGroups[id].ReadAsync(config.ID);
        }
        /// <summary>
        /// 异步读
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="callBacck"></param>
        public static void ReadAsync(DC_DATA_CONFIG config, Action<object> callBack)
        {
            Task.Run(() =>
            {
                return Read(config);
            }).ContinueWith(s => callBack?.Invoke(s.Result));
        }
        /// <summary>
        /// 异步写
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <returns></returns>
        public static void WriteAsync(DC_DATA_CONFIG config, object value)
        {
            if (config.DEVICE_ID == null)
            {
                throw new NullReferenceException("DEVICE_ID为空，无法进行读写");
            }
            int id = (int)config.DEVICE_ID;
            OpcServer.Instance.OpcGroups[id].WriteAsync(config.ID, value);
        }
        /// <summary>
        /// 异步写
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <param name="value"></param>
        /// <param name="callBack"></param>
        public static void WriteAsync(DC_DATA_CONFIG config, dynamic value, Action<bool> callBack)
        {
            Task.Run(() =>
            {
                return Write(config, value);
            }).ContinueWith(s => callBack?.Invoke(s.Result));
        }
    }
}
