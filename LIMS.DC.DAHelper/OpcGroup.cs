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
    public delegate void DataReceivedEventHandler(int dataID, object value, string quality, DateTime stamp);

    public class OpcGroup : IOPCDataCallback
    {
        public OpcGroup(int clientGroupHandle)
        {
            ClientGroupHandle = clientGroupHandle;
        }
        public OpcGroup(string groupName, int clientGroupHandle)
        {
            GroupName = groupName;
            ClientGroupHandle = clientGroupHandle;
        }
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int IsActive { get; set; } = 1;
        /// <summary>
        /// 请求的更新速率
        /// </summary>
        public int RequestedUpdateRate { get; set; } = 500;
        /// <summary>
        /// 客户端组句柄
        /// </summary>
        public int ClientGroupHandle { get; set; }
        /// <summary>
        /// 输出参数，服务器组句柄
        /// </summary>
        public int ServerGroupHandle;

        private bool _isSubscribed;
        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool IsSubscribed
        {
            get { return _isSubscribed; }
            set
            {
                _isSubscribed = value;
                SetOPCSubscribe(_isSubscribed);
            }
        }
        /// <summary>
        /// 输出参数，组对象
        /// </summary>
        public object ObjGroup;

        private IOPCSyncIO oPCSyncIO;
        /// <summary>
        /// 同步读写对象
        /// </summary>
        public IOPCSyncIO OPCSyncIO
        {
            get
            {
                if (oPCSyncIO == null && ObjGroup != null) oPCSyncIO = (IOPCSyncIO)ObjGroup;
                return oPCSyncIO;
            }
        }

        private IOPCAsyncIO2 oPCAsyncIO;
        /// <summary>
        /// 异步读写对象
        /// </summary>
        public IOPCAsyncIO2 OPCAsyncIO
        {
            get
            {
                if (oPCAsyncIO == null && ObjGroup != null) oPCAsyncIO = (IOPCAsyncIO2)ObjGroup;
                return oPCAsyncIO;
            }
        }

        private IOPCGroupStateMgt oPCGroupStateMgt;
        /// <summary>
        /// 组管理对象
        /// </summary>
        public IOPCGroupStateMgt OPCGroupStateMgt
        {
            get
            {
                if (oPCGroupStateMgt == null && ObjGroup != null) oPCGroupStateMgt = (IOPCGroupStateMgt)ObjGroup;
                return oPCGroupStateMgt;
            }
        }

        private IConnectionPointContainer connectionPointContainer;
        /// <summary>
        /// 组的异步调用连接
        /// </summary>
        public IConnectionPointContainer ConnectionPointContainer
        {
            get
            {
                if (connectionPointContainer == null && ObjGroup != null) connectionPointContainer = (IConnectionPointContainer)ObjGroup;
                return connectionPointContainer;
            }
        }

        private IOPCItemMgt oPCItemMgt;
        /// <summary>
        /// 项管理对象
        /// </summary>
        public IOPCItemMgt OPCItemMgt
        {
            get
            {
                if (oPCItemMgt == null && ObjGroup != null) oPCItemMgt = (IOPCItemMgt)ObjGroup;
                return oPCItemMgt;
            }
        }
        /// <summary>
        /// 输出参数，OPCServer的连接点
        /// </summary>
        public IConnectionPoint ConnectionPoint;
        /// <summary>
        /// 输出参数
        /// </summary>
        public int dwCookie;
        /// <summary>
        /// 编码：英语
        /// </summary>
        public int LocalId { get; set; } = 0x407;

        public Dictionary<int, OpcItem> OpcItems { get; set; } = new Dictionary<int, OpcItem>();
        /// <summary>
        /// 添加一项
        /// </summary>
        /// <param name="opcItem"></param>
        public void AddItem(OpcItem opcItem)
        {
            OPCITEMDEF item = new OPCITEMDEF();
            item.szAccessPath = opcItem.AccessPath;
            item.szItemID = opcItem.PlcAddress;
            item.bActive = opcItem.IsActive;
            item.hClient = opcItem.ClientItemHandle;
            item.dwBlobSize = opcItem.dwBlobSize;
            item.pBlob = opcItem.pBlob;
            item.vtRequestedDataType = opcItem.RequestedDataType;
            IntPtr pAddResults = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                OPCItemMgt.AddItems(1, new OPCITEMDEF[] { item }, out pAddResults, out pErrors);
                //获取ServerItemrHandles
                int[] errors = new int[1];
                Marshal.Copy(pErrors, errors, 0, 1);
                IntPtr pos = pAddResults;
                if (errors[0] == 0)
                {
                    OPCITEMRESULT result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                    opcItem.ServerItemHandle = result.hServer;
                    OpcItems.Add(opcItem.ClientItemHandle, opcItem);
                }
                else
                {
                    throw new ApplicationException($"添加组[{GroupName}]时，添加项[{item.szItemID}]失败！");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pAddResults != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pAddResults);
                    pAddResults = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="lstOpcItem"></param>
        public void AddItems(List<OpcItem> lstOpcItem)
        {
            OPCITEMDEF[] itemArray = new OPCITEMDEF[lstOpcItem.Count];
            for (int i = 0; i < itemArray.Length; i++)
            {
                itemArray[i].szAccessPath = lstOpcItem[i].AccessPath;
                itemArray[i].szItemID = lstOpcItem[i].PlcAddress;
                itemArray[i].bActive = lstOpcItem[i].IsActive;
                itemArray[i].hClient = lstOpcItem[i].ClientItemHandle;
                itemArray[i].dwBlobSize = lstOpcItem[i].dwBlobSize;
                itemArray[i].pBlob = lstOpcItem[i].pBlob;
                itemArray[i].vtRequestedDataType = lstOpcItem[i].RequestedDataType;
            }
            IntPtr pAddResults = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                OPCItemMgt.AddItems(itemArray.Length, itemArray, out pAddResults, out pErrors);
                //获取ServerItemrHandles
                int[] errors = new int[itemArray.Length];
                Marshal.Copy(pErrors, errors, 0, itemArray.Length);
                IntPtr pos = pAddResults;
                for (int i = 0; i < errors.Length; i++)
                {
                    if (errors[i] == 0)
                    {
                        if (i != 0)
                        {
                            pos = new IntPtr(pos.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                        }
                        OPCITEMRESULT result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                        lstOpcItem[i].ServerItemHandle = result.hServer;
                        OpcItems.Add(lstOpcItem[i].ClientItemHandle, lstOpcItem[i]);
                    }
                    else
                    {
                        throw new ApplicationException($"添加组[{GroupName}]时，添加项[{itemArray[i].szItemID}]失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pAddResults != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pAddResults);
                    pAddResults = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="isSubscribe">是否订阅</param>
        private void SetOPCSubscribe(bool isSubscribe)
        {
            IntPtr pRequestedUpdateRate = IntPtr.Zero;
            int pRevisedUpdateRate;
            int nActive = 0;
            GCHandle hActive = GCHandle.Alloc(nActive, GCHandleType.Pinned);
            hActive.Target = isSubscribe ? 1 : 0;
            IntPtr pTimeBias = IntPtr.Zero;
            IntPtr pDeadBand = IntPtr.Zero;
            IntPtr pLCID = IntPtr.Zero;
            IntPtr phClientGroup = IntPtr.Zero;
            try
            {
                OPCGroupStateMgt.SetState(pRequestedUpdateRate, out pRevisedUpdateRate, hActive.AddrOfPinnedObject(), pTimeBias,
                    pDeadBand, pLCID, phClientGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                hActive.Free();
            }
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (dwCookie != 0)
                {
                    ConnectionPoint.Unadvise(dwCookie);
                    dwCookie = 0;
                }
            }
            catch (Exception)
            {
                dwCookie = 0;
            }
            Marshal.ReleaseComObject(ConnectionPoint);
            ConnectionPoint = null;
            Marshal.ReleaseComObject(connectionPointContainer);
            connectionPointContainer = null;
            if (oPCSyncIO != null)
            {
                Marshal.ReleaseComObject(oPCSyncIO);//减与指定的 COM 对象关联的运行时可调用包装器 (RCW) 的引用计数。
                oPCSyncIO = null;
            }
            if (oPCAsyncIO != null)
            {
                Marshal.ReleaseComObject(oPCAsyncIO);//减与指定的 COM 对象关联的运行时可调用包装器 (RCW) 的引用计数。
                oPCAsyncIO = null;
            }
            if (oPCGroupStateMgt != null)
            {
                Marshal.ReleaseComObject(oPCGroupStateMgt);
                oPCGroupStateMgt = null;
            }
            if (oPCItemMgt != null)
            {
                Marshal.ReleaseComObject(oPCItemMgt);
                oPCItemMgt = null;
            }
            if (ObjGroup != null)
            {
                Marshal.ReleaseComObject(ObjGroup);
                ObjGroup = null;
            }
        }

        /// <summary>
        /// 同步读
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <returns></returns>
        public object Read(int dataPointID)
        {
            IntPtr itemValues = IntPtr.Zero;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                OPCSyncIO.Read(OPCDATASOURCE.OPC_DS_DEVICE, 1,
                       new int[] { OpcItems[dataPointID].ServerItemHandle }, out itemValues, out pErrors);
                int[] error = new int[1];
                Marshal.Copy(pErrors, error, 0, error.Length);
                if (error[0] == 0)
                {
                    OPCITEMSTATE itemState = (OPCITEMSTATE)Marshal.PtrToStructure(itemValues, typeof(OPCITEMSTATE));
                    return itemState.vDataValue;
                }
                else
                {
                    throw new ApplicationException($"数据点(ID:{dataPointID})同步读取失败。");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (itemValues != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(itemValues);
                    itemValues = IntPtr.Zero;
                }
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// 同步写
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(int dataPointID, object value)
        {
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                object[] obj = new object[] { value };
                OPCSyncIO.Write(1, new int[] { OpcItems[dataPointID].ServerItemHandle }, obj, out pErrors);
                int[] errors = new int[1];
                Marshal.Copy(pErrors, errors, 0, 1);
                if (errors[0] != 0)
                {
                    throw new ApplicationException($"数据点(ID:{dataPointID})同步写入失败。");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// 异步读
        /// </summary>
        /// <param name="dataPointID"></param>
        public void ReadAsync(int dataPointID)
        {
            int cancelId;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                OPCAsyncIO.Read(1, new int[] { OpcItems[dataPointID].ServerItemHandle },
                     OpcItems[dataPointID].ClientItemHandle, out cancelId, out pErrors);
                int[] errors = new int[1];
                Marshal.Copy(pErrors, errors, 0, 1);
                if (errors[0] != 0)
                {
                    throw new ApplicationException($"数据点(ID:{dataPointID})异步读取失败。");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// 异步写
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <param name="value"></param>
        public void WriteAsync(int dataPointID, object value)
        {
            int cancelId;
            IntPtr pErrors = IntPtr.Zero;
            try
            {
                OPCAsyncIO.Write(1, new int[] { OpcItems[dataPointID].ServerItemHandle },
                new object[] { value }, OpcItems[dataPointID].ClientItemHandle, out cancelId, out pErrors);
                int[] errors = new int[1];
                Marshal.Copy(pErrors, errors, 0, 1);
                if (errors[0] != 0)
                {
                    throw new ApplicationException($"数据点(ID:{dataPointID})异步写入失败。");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
        }
        /// <summary>
        /// 获取质量
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        private string GetQuality(int quality)
        {
            string strQuality = string.Empty;
            switch (quality)
            {
                case 0:
                    strQuality = "bad";
                    break;
                case 192:
                    strQuality = "good";
                    break;
                default:
                    strQuality = "bad";
                    break;
            }
            return strQuality;
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="ft"></param>
        /// <returns></returns>
        private DateTime ToDateTime(OpcRcw.Da.FILETIME ft)
        {
            long highbuf = (long)ft.dwHighDateTime;
            long buffer = (highbuf << 32) + ft.dwLowDateTime;
            return DateTime.FromFileTimeUtc(buffer).AddHours(8.0);
        }
        /// <summary>
        /// 将数组转化为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string GetString(Array bytes)
        {
            List<string> lst = new List<string>();
            foreach (var item in bytes)
            {
                lst.Add(item.ToString());
            }
            return string.Join(",", lst);
        }

        /// <summary>
        /// 接收数据事件
        /// </summary>
        public event DataReceivedEventHandler DataReceived;

        /// <summary>
        /// 订阅接收数据
        /// </summary>
        /// <param name="dwTransid"></param>
        /// <param name="hGroup"></param>
        /// <param name="hrMasterquality"></param>
        /// <param name="hrMastererror"></param>
        /// <param name="dwCount"></param>
        /// <param name="phClientItems"></param>
        /// <param name="pvValues"></param>
        /// <param name="pwQualities"></param>
        /// <param name="pftTimeStamps"></param>
        /// <param name="pErrors"></param>
        public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            string[] qualities = new string[dwCount];
            DateTime[] timeStamps = new DateTime[dwCount];
            for (int i = 0; i < dwCount; i++)
            {
                qualities[i] = GetQuality(pwQualities[i]);
                timeStamps[i] = ToDateTime(pftTimeStamps[i]);
                if (pvValues[i] is Array)
                {
                    pvValues[i] = GetString((Array)pvValues[i]);
                }
                DataReceived?.Invoke(phClientItems[i], pvValues[i], qualities[i], timeStamps[i]);
            }
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {

        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {

        }

        public void OnCancelComplete(int dwTransid, int hGroup)
        {

        }
    }
}
