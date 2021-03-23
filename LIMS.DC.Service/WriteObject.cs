using LIMS.DC.DAL;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    public class WriteObject
    {
        public DC_DATA_CONFIG Config { get; set; }

        DC_Service dC_Service = new DC_Service();

        public IPLCOperate Operator { get; set; }

        public void Request(List<DC_WRITE_DATA> datas)
        {
            DC_WRITE_DATA wData = null;
            foreach (var data in datas)
            {
                if (data.DATA_CONFIG_ID == Config.ID)
                {
                    wData = data;
                    break;
                }
            }
            if (wData != null)
            {
                Operator.Write(Config, wData.VALUE);//写入plc

                dC_Service.UpdateWriteDataFlag(wData.ID);//更新标志
            }
        }
    }
}
