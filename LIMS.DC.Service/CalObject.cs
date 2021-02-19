using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    class CalObject
    {
        public IPLCOperate Operator { get; set; }

        /// <summary>
        /// 校正时间
        /// </summary>
        public DC_DATA_CONFIG CAL_TIME { get; set; }
        /// <summary>
        /// 写入标志
        /// </summary>
        public DC_DATA_CONFIG CAL_WRITE_FLAG { get; set; }

        public bool Request()
        {
            bool res= Operator.Write(CAL_TIME, DateTime2ByteArray(DateTime.Now));
            if(res)
            {
                return Operator.Write(CAL_WRITE_FLAG,1);
            }
            return false;
        }

        /// <summary>
        /// 将时间转化为byte数组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static byte[] DateTime2ByteArray(DateTime dt)
        {
            byte[] byteArray = new byte[6];
            byteArray[0] = Convert.ToByte(dt.Year.ToString().Substring(2, 2));
            byteArray[1] = (byte)dt.Month;
            byteArray[2] = (byte)dt.Day;
            byteArray[3] = (byte)dt.Hour;
            byteArray[4] = (byte)dt.Minute;
            byteArray[5] = (byte)dt.Second;
            return byteArray;
        }
    }
}
