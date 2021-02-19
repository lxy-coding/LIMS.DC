using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.DC.DAL;
using LIMS.DC.Model;
namespace LIMS.DC.Service
{
    class RealObject
    {
        public string User { get; set; }

        public string Table { get; set; }

        public string IdentityValue { get; set; }

        public List<DC_DATA_CONFIG> Configs { get; set; }

        DC_Service dC_Service = new DC_Service();

        public void Request(List<DC_REAL_DATA> datas)
        {
            Dictionary<DC_DATA_CONFIG, DC_REAL_DATA> dic = new Dictionary<DC_DATA_CONFIG, DC_REAL_DATA>();
            foreach (var data in datas)
            {
                foreach (var config in Configs)
                {
                    if(data.DATA_CONFIG_ID==config.ID)
                    {
                        dic.Add(config, data);
                    }
                }
            }
            if(dic.Count>0)
            {
                dC_Service.UpdateRealData2BusiTable(User,Table,IdentityValue,dic);//更新至业务表

                dC_Service.UpdateRealDataFlag(dic.Values.Select(s=>s.ID).ToList());//更新标志
            }
        }
    }
}
