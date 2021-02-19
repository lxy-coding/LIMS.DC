using OpcRcw.Comn;
using OpcRcw.Da;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.DAHelper
{
    /// <summary>
    /// OpcServer停止工作委托
    /// </summary>
    /// <param name="reason"></param>
    public delegate void ServerShutDownEventHandler(string reason);
    public class OpcServer : IOPCShutdown
    {
        /// <summary>
        /// 私有化构造函数
        /// </summary>
        private OpcServer() { }
        private static readonly object _objLock = new object();
        private static OpcServer _opcServer = null;

        public static OpcServer Instance
        {
            get
            {
                if (_opcServer == null)
                {
                    lock (_objLock)
                    {
                        if (_opcServer == null)
                        {
                            _opcServer = new OpcServer();
                        }
                    }
                }
                return _opcServer;
            }
        }
        /// <summary>
        /// OpcServer停止工作事件
        /// </summary>
        public event ServerShutDownEventHandler ServerShutDown;
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool Connected
        {
            get { return opcServer == null ? false : true; }
        }

        private int _connectStateClientItemHandle = -1;
        public int ConnectStateClientItemHandle
        {
            get { return --_connectStateClientItemHandle; }
        }
        /// <summary>
        /// OpcServer
        /// </summary>
        public IOPCServer opcServer;
        /// <summary>
        /// 连接点容器
        /// </summary>
        private IConnectionPointContainer connectionPointContainer;
        /// <summary>
        /// 输出参数，OPCServer的连接点
        /// </summary>
        private IConnectionPoint connectionPoint;
        /// <summary>
        /// 输出参数
        /// </summary>
        private int dwCookie;

        public Dictionary<int, OpcGroup> OpcGroups { get; set; } = new Dictionary<int, OpcGroup>();
        /// <summary>
        /// 组集合
        /// </summary>
        //private List<OpcGroup> _lstOpcGroup = new List<OpcGroup>();
        /// <summary>
        /// 连接OpcServer
        /// </summary>
        /// <param name="opcServerName"></param>
        /// <param name="serverIP"></param>
        public bool OpcConnect(string opcServerName, string serverIP)
        {
            if (!string.IsNullOrEmpty(serverIP) && !string.IsNullOrEmpty(opcServerName))
            {
                Type type = Type.GetTypeFromProgID(opcServerName, serverIP);
                if (type != null)
                {
                    opcServer = (IOPCServer)Activator.CreateInstance(type);
                    connectionPointContainer = (IConnectionPointContainer)opcServer;
                    Guid iid = typeof(IOPCShutdown).GUID; //为所有的异步调用创建回调
                    connectionPointContainer.FindConnectionPoint(ref iid, out connectionPoint); //为 OPC Server 的 连 接 点 与 客 户 端 接 收 点 之 间 建 立 连 接
                    connectionPoint.Advise(this, out dwCookie);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 断开连接OpcServer
        /// </summary>
        public void OpcDisConnect()
        {
            foreach (var item in OpcGroups)
            {
                try
                {
                    item.Value.Dispose();
                    opcServer.RemoveGroup(item.Value.ServerGroupHandle, 0);
                }
                catch (Exception)
                {

                }
            }
            try
            {
                if (dwCookie != 0)
                {
                    connectionPoint.Unadvise(dwCookie);
                    dwCookie = 0;
                }
            }
            catch (Exception)
            {
                dwCookie = 0;
            }
            try
            {
                if (connectionPoint != null)
                {
                    Marshal.ReleaseComObject(connectionPoint);
                    connectionPoint = null;
                }
                if (connectionPointContainer != null)
                {
                    Marshal.ReleaseComObject(connectionPointContainer);
                    connectionPointContainer = null;
                }
                if (opcServer != null)
                {
                    Marshal.ReleaseComObject(opcServer);//减与指定的 COM 对象关联的运行时可调用包装器 (RCW) 的引用计数。
                }
            }
            finally
            {
                if (opcServer != null)
                {
                    opcServer = null;
                }
                if (_opcServer != null)
                {
                    _opcServer = null;
                }
            }
        }
        /// <summary>
        /// 默认的组名称
        /// </summary>
        private string _defaultGroupName = "Group1";
        /// <summary>
        /// 默认的客户端组句柄
        /// </summary>
        private int _defaultClientGroupHandle = 100000;
        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="opcGroup"></param>
        public void AddGroup(OpcGroup opcGroup)
        {
            GCHandle hTimeBias = GCHandle.Alloc(0, GCHandleType.Pinned);//为指定的对象分配句柄。
            GCHandle hPercendDeadBand = GCHandle.Alloc(0, GCHandleType.Pinned);
            int updateRate;
            Guid riid = typeof(IOPCItemMgt).GUID;
            if (string.IsNullOrEmpty(opcGroup.GroupName))//组名称为空，给个默认的组名
            {
                opcGroup.GroupName = _defaultGroupName;
                _defaultGroupName = "Group" + (Convert.ToInt32(_defaultGroupName.Remove(0, 5)) + 1);
            }
            //if (opcGroup.ClientGroupHandle == 0)
            //{
            //    opcGroup.ClientGroupHandle = _defaultClientGroupHandle;
            //    _defaultClientGroupHandle++;
            //}
            try
            {
                opcServer.AddGroup(opcGroup.GroupName, opcGroup.IsActive, opcGroup.RequestedUpdateRate, opcGroup.ClientGroupHandle,
                   hTimeBias.AddrOfPinnedObject(), hPercendDeadBand.AddrOfPinnedObject(), opcGroup.LocalId, out opcGroup.ServerGroupHandle,
                   out updateRate, ref riid, out opcGroup.ObjGroup);
                // _lstOpcGroup.Add(opcGroup);
                OpcGroups.Add(opcGroup.ClientGroupHandle, opcGroup);
                Guid iid = typeof(IOPCDataCallback).GUID; //为所有的异步调用创建回调
                opcGroup.ConnectionPointContainer.FindConnectionPoint(ref iid, out opcGroup.ConnectionPoint); //为OPCServer的连接点与客户端接收点之间建立连接
                opcGroup.ConnectionPoint.Advise(opcGroup, out opcGroup.dwCookie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (hTimeBias.IsAllocated) hTimeBias.Free();
                if (hPercendDeadBand.IsAllocated) hPercendDeadBand.Free();
            }
        }
        /// <summary>
        /// 服务器停止工作
        /// </summary>
        /// <param name="szReason"></param>
        public void ShutdownRequest(string szReason)
        {
            ServerShutDown?.BeginInvoke(szReason, null, null);
        }
    }
}
