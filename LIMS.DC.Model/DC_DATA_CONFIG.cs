using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.Model
{
    public class DC_DATA_CONFIG
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public int? DEVICE_ID { get; set; }
        /// <summary>
        /// 编号;
        /// </summary>
        public string NUM { get; set; }
        /// <summary>
        /// 名称;
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 内存地址;
        /// </summary>
        public string MEMORY_ADDRESS { get; set; }
        /// <summary>
        /// 是否订阅;0：不订阅；1：订阅
        /// </summary>
        public int? SUBSCRIPTION { get; set; }
        /// <summary>
        /// 表所属用户
        /// <summary>
        public string TABLE_USER { get; set; }
        /// <summary>
        /// 表名称;
        /// <summary>
        public string TABLE_NAME { get; set; }
        /// <summary>
        /// 标识列;
        /// <summary>
        public string IDENTITY_COLUMN { get; set; }
        /// <summary>
        /// 标识值;
        /// <summary>
        public string IDENTITY_VALUE { get; set; }
        /// <summary>
        /// 字段名;
        /// <summary>
        public string FIELD_NAME { get; set; }
        /// <summary>
        /// 字段数据类型;
        /// <summary>
        public string FIELD_DATA_TYPE { get; set; }
        /// <summary>
        /// 字段数据长度(VARCHAR2有效)
        /// <summary>
        public int? FIELD_DATA_LENGTH { get; set; }
        /// <summary>
        /// 字段精度(NUMBER有效)
        /// </summary>
        public int? FIELD_DATA_PRECISION { get; set; }
        /// <summary>
        /// 字段小数位数(NUMBER有效)
        /// </summary>
        public int? FIELD_DATA_SCALE { get; set; }
        /// <summary>
        /// 转换;
        /// </summary>
        public string CONVERTER { get; set; }
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
    }
}
