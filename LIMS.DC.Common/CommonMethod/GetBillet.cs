using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Common.CommonMethod
{
    public class GetBillet
    {
        public static string Query(int orderNum)
        {
            return string.Format("QM{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), orderNum);
        }

        public static string QueryHeatNum(int orderNum)
        {
            return string.Format("HM{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), orderNum);
        }

        public static int GetRotateZ(int zValue)
        {
            int ratateZ = 0;
            if (zValue >= 0 && zValue < 45)
            {
                ratateZ = 0;
            }
            if (zValue >= 45 && zValue < 135)
            {
                ratateZ = 1;
            }
            if (zValue >= 135 && zValue <= 180)
            {
                ratateZ = 0;
            }
            return ratateZ;
        }
    }
}
