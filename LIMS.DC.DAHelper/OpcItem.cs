using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.DAHelper
{
    /// <summary>
    /// OpcItem
    /// </summary>
    public class OpcItem
    {
        public OpcItem(int clientItemHandle, string plcAddress)
        {
            ClientItemHandle = clientItemHandle;
            PlcAddress = plcAddress;
        }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int IsActive { get; set; } = 1;
        /// <summary>
        /// 客户端项句柄
        /// </summary>
        public int ClientItemHandle { get; set; }
        /// <summary>
        /// 服务器项句柄
        /// </summary>
        public int ServerItemHandle { get; set; }
        /// <summary>
        /// plc完整地址
        /// </summary>
        public string PlcAddress { get; set; }
        /// <summary>
        /// 请求的数据类型,8：字符型,0:item-self defined
        /// </summary>
        public short RequestedDataType { get; set; } = 0;

        public string AccessPath { get; set; } = "";

        public int dwBlobSize { get; set; } = 0;

        public IntPtr pBlob { get; set; } = IntPtr.Zero;
    }
}
