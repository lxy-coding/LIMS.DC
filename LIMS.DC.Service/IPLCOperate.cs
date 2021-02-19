using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Service
{
    public interface IPLCOperate
    {
        object Read(DC_DATA_CONFIG config);

        bool Write(DC_DATA_CONFIG config, object value);
    }
}
