using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Model
{
    public class DC_SERVER
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 编号;
        /// </summary>
        public string NUM { get; set; }
        /// <summary>
        /// 名称;
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// IP地址;
        /// </summary>
        public string IP_ADDRESS { get; set; }
        /// <summary>
        /// 描述;
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 启用;0未启用；1启用
        /// </summary>
        public int? ENABLE { get; set; }
        /// <summary>
        /// 预留字段1;
        /// <summary>
        public string FIELD1 { get; set; }
        /// <summary>
        /// 预留字段2;
        /// <summary>
        public string FIELD2 { get; set; }
        /// <summary>
        /// 预留字段3;
        /// <summary>
        public string FIELD3 { get; set; }
        /// <summary>
        /// 设备集合
        /// </summary>
        public ObservableCollection<DC_DEVICE> Devices { get; set; }
    }
}
