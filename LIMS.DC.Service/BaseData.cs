using LIMS.DC.DAL;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    class BaseData
    {
        static BaseData()
        {
            try
            {
                string serverNum = System.Configuration.ConfigurationManager.AppSettings["ServerNum"];
                DC_Service  dC_Service = new DC_Service();
                Server = dC_Service.GetDCServer(serverNum);
                Devices = dC_Service.GetDCDevices(Server, 1);
                Configs= dC_Service.GetDCDataConfigs(Devices.Select(s=>s.ID).ToList(), 1);
            }
            catch (Exception)
            {

            }
        }

        public static DC_SERVER Server { get; set; }

        public static List<DC_DEVICE> Devices { get; set; }

        public static List<DC_DATA_CONFIG> Configs { get; set; }
    }
}
