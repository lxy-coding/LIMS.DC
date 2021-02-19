using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Model
{
    public class DC_DEVICE
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 服务器ID
        /// </summary>
        public int? SERVER_ID { get; set; }
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
        /// MAC地址;
        /// </summary>
        public string MAC_ADDRESS { get; set; }
        /// <summary>
        /// 通道;
        /// </summary>
        public string CHANNEL { get; set; } = "S7";
        /// <summary>
        /// 连接;
        /// </summary>
        public string S7CONNECTION { get; set; }
        /// <summary>
        /// 天车ID;如果不是天车，则为空
        /// </summary>
        public int? CRA_ID { get; set; }
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
        /// 数据配置集合
        /// </summary>
        public ObservableCollection<DC_DATA_CONFIG> Configs { get; set; }
    }
}
