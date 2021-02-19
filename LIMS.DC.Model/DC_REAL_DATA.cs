using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Model
{
    public class DC_REAL_DATA
    {
        /// <summary>
        /// 主键ID;
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// 数据点表ID;
        /// <summary>
        public int? DATA_CONFIG_ID { get; set; }
        /// <summary>
        /// 值;
        /// <summary>
        public string VALUE { get; set; }
        /// <summary>
        /// 质量;
        /// <summary>
        public string QUALITY { get; set; }
        /// <summary>
        /// 时间戳;
        /// <summary>
        public DateTime? TIME_STAMP { get; set; }
        /// <summary>
        /// 更新时间;
        /// <summary>
        public DateTime? UPDATE_TIME { get; set; }
        /// <summary>
        /// 是否更新;0：未更新；1：已更新
        /// <summary>
        public int? UPDATED { get; set; }
        /// <summary>
        /// 读取时间;
        /// <summary>
        public DateTime? READ_TIME { get; set; }
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
    }
}
