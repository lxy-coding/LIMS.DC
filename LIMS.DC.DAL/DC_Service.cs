using LIMS.DC.DAL.Helper;
using LIMS.DC.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using StringToLambda;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.DC.DAL
{
    public class DC_Service
    {
        #region DC_SERVER
        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="serverNum"></param>
        /// <returns></returns>
        public DC_SERVER GetDCServer(string serverNum)
        {
            string sql = $"SELECT * FROM DC.DC_SERVER WHERE NUM='{serverNum}'";
            OracleDataReader reader = OracleDataHelper.ExecuteReader(sql);
            DC_SERVER server = null;
            if (reader.Read())
            {
                server = new DC_SERVER()
                {
                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                    ENABLE = GetValue(reader, "ENABLE"),
                    ID = Convert.ToInt32(reader["ID"]),
                    IP_ADDRESS = reader["IP_ADDRESS"].ToString(),
                    NAME = reader["NAME"].ToString(),
                    NUM = reader["NUM"].ToString(),
                    FIELD1 = reader["FIELD1"].ToString(),
                    FIELD2 = reader["FIELD2"].ToString(),
                    FIELD3 = reader["FIELD3"].ToString(),
                };
            }
            reader.Close();
            return server;
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="serverNum"></param>
        /// <returns></returns>
        public List<DC_SERVER> GetDCServers()
        {
            string sql = $"SELECT * FROM DC.DC_SERVER";
            OracleDataReader reader = OracleDataHelper.ExecuteReader(sql);
            List<DC_SERVER> lst = new List<DC_SERVER>();
            while (reader.Read())
            {
                DC_SERVER server = new DC_SERVER()
                {
                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                    ENABLE = GetValue(reader, "ENABLE"),
                    ID = Convert.ToInt32(reader["ID"]),
                    IP_ADDRESS = reader["IP_ADDRESS"].ToString(),
                    NAME = reader["NAME"].ToString(),
                    NUM = reader["NUM"].ToString(),
                    FIELD1 = reader["FIELD1"].ToString(),
                    FIELD2 = reader["FIELD2"].ToString(),
                    FIELD3 = reader["FIELD3"].ToString(),
                };
                lst.Add(server);
            }
            reader.Close();
            return lst;
        }

        public int InsertDCServer(DC_SERVER server)
        {
            string sql = $@"INSERT INTO DC.DC_SERVER(
NUM, 
NAME, 
IP_ADDRESS, 
DESCRIPTION, 
ENABLE
)VALUES(
{GetValueStr(server.NUM)},
{GetValueStr(server.NAME)},
{GetValueStr(server.IP_ADDRESS)},
{GetValueStr(server.DESCRIPTION)},
{GetValueStr(server.ENABLE)}
) RETURNING ID INTO :ID";
            OracleParameter para = new OracleParameter("ID", OracleDbType.Int32);
            int res = OracleDataHelper.ExecuteNonQuery(sql, new OracleParameter[] { para });
            server.ID = ((OracleDecimal)para.Value).ToInt32();
            return res;
        }

        public int ModifyDCServer(DC_SERVER server)
        {
            string sql = $@"UPDATE DC.DC_SERVER SET
NUM={GetValueStr(server.NUM)},
NAME={GetValueStr(server.NAME)},
IP_ADDRESS={GetValueStr(server.IP_ADDRESS)},
DESCRIPTION={GetValueStr(server.DESCRIPTION)},
ENABLE={GetValueStr(server.ENABLE)}
WHERE ID={GetValueStr(server.ID)}";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }

        public int DeleteDCServer(DC_SERVER server)
        {
            string sql = $@"DELETE FROM DC.DC_SERVER WHERE ID={GetValueStr(server.ID)}";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }
        #endregion


        #region DC_DEVICE
        /// <summary>
        /// 获取设备集合
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        private List<DC_DEVICE> GetDCDevices(string whereSql)
        {
            string sql = $"SELECT * FROM DC.DC_DEVICE WHERE 1=1 {whereSql}";
            OracleDataReader reader = OracleDataHelper.ExecuteReader(sql);
            List<DC_DEVICE> lst = new List<DC_DEVICE>();
            while (reader.Read())
            {
                DC_DEVICE device = new DC_DEVICE()
                {
                    CHANNEL = reader["CHANNEL"].ToString(),
                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                    ENABLE = GetValue(reader, "ENABLE"),
                    ID = Convert.ToInt32(reader["ID"]),
                    IP_ADDRESS = reader["IP_ADDRESS"].ToString(),
                    MAC_ADDRESS = reader["MAC_ADDRESS"].ToString(),
                    NAME = reader["NAME"].ToString(),
                    NUM = reader["NUM"].ToString(),
                    S7CONNECTION = reader["S7CONNECTION"].ToString(),
                    SERVER_ID = GetValue(reader, "SERVER_ID"),
                    CRA_ID = GetValue(reader, "CRA_ID"),
                    FIELD1 = reader["FIELD1"].ToString(),
                    FIELD2 = reader["FIELD2"].ToString(),
                    FIELD3 = reader["FIELD3"].ToString(),
                };
                lst.Add(device);
            }
            reader.Close();
            return lst;
        }
        /// <summary>
        /// 获取设备集合
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public List<DC_DEVICE> GetDCDevices(DC_SERVER server)
        {
            return GetDCDevices($" AND SERVER_ID={server.ID}");
        }
        /// <summary>
        /// 获取设备集合
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public List<DC_DEVICE> GetDCDevices(DC_SERVER server, int enable)
        {
            return GetDCDevices($" AND SERVER_ID={server.ID} AND ENABLE={enable}");
        }

        public int InsertDCDevice(DC_DEVICE device)
        {
            string sql = $@"INSERT INTO DC.DC_DEVICE(
SERVER_ID, 
NUM, 
NAME, 
IP_ADDRESS, 
MAC_ADDRESS, 
CHANNEL, 
S7CONNECTION, 
DESCRIPTION, 
ENABLE,
CRA_ID)VALUES(
{GetValueStr(device.SERVER_ID)},
{GetValueStr(device.NUM)},
{GetValueStr(device.NAME)},
{GetValueStr(device.IP_ADDRESS)},
{GetValueStr(device.MAC_ADDRESS)},
{GetValueStr(device.CHANNEL)},
{GetValueStr(device.S7CONNECTION)},
{GetValueStr(device.DESCRIPTION)},
{GetValueStr(device.ENABLE)},
{GetValueStr(device.CRA_ID)}
) RETURNING ID INTO :ID";
            OracleParameter para = new OracleParameter("ID", OracleDbType.Int32);
            int res = OracleDataHelper.ExecuteNonQuery(sql, new OracleParameter[] { para });
            device.ID = ((OracleDecimal)para.Value).ToInt32();
            return res;
        }

        public int ModifyDevice(DC_DEVICE device)
        {
            string sql = $@"UPDATE DC.DC_DEVICE SET
SERVER_ID={GetValueStr(device.SERVER_ID)},
NUM={GetValueStr(device.NUM)},
NAME={GetValueStr(device.NAME)},
IP_ADDRESS={GetValueStr(device.IP_ADDRESS)},
MAC_ADDRESS={GetValueStr(device.MAC_ADDRESS)},
CHANNEL={GetValueStr(device.CHANNEL)},
S7CONNECTION={GetValueStr(device.S7CONNECTION)},
DESCRIPTION={GetValueStr(device.DESCRIPTION)},
ENABLE={GetValueStr(device.ENABLE)},
CRA_ID={GetValueStr(device.CRA_ID)}
WHERE ID={GetValueStr(device.ID)}";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }

        public int DeleteDevice(DC_DEVICE device)
        {
            string sql = $@"DELETE FROM DC.DC_DEVICE WHERE ID={GetValueStr(device.ID)}";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }
        #endregion


        #region DC_DATA_CONFIG
        /// <summary>
        /// 获取数据点配置集合
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public List<DC_DATA_CONFIG> GetDCDataConfigs(string whereSql)
        {
            string sql = $"SELECT * FROM DC.DC_DATA_CONFIG WHERE 1=1 {whereSql}";
            OracleDataReader reader = OracleDataHelper.ExecuteReader(sql);
            List<DC_DATA_CONFIG> lst = new List<DC_DATA_CONFIG>();
            while (reader.Read())
            {
                DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                {
                    DESCRIPTION = reader["DESCRIPTION"].ToString(),
                    ENABLE = GetValue(reader, "ENABLE"),
                    ID = Convert.ToInt32(reader["ID"]),
                    NAME = reader["NAME"].ToString(),
                    NUM = reader["NUM"].ToString(),
                    DEVICE_ID = GetValue(reader, "DEVICE_ID"),
                    CONVERTER = GetValue(reader, "CONVERTER"),
                    FIELD_DATA_LENGTH = GetValue(reader, "FIELD_DATA_LENGTH"),
                    FIELD_DATA_PRECISION = GetValue(reader, "FIELD_DATA_PRECISION"),
                    FIELD_DATA_SCALE = GetValue(reader, "FIELD_DATA_SCALE"),
                    FIELD_DATA_TYPE = GetValue(reader, "FIELD_DATA_TYPE"),
                    TABLE_NAME = GetValue(reader, "TABLE_NAME"),
                    TABLE_USER = GetValue(reader, "TABLE_USER"),
                    IDENTITY_VALUE = GetValue(reader, "IDENTITY_VALUE"),
                    IDENTITY_COLUMN = GetValue(reader, "IDENTITY_COLUMN"),
                    FIELD_NAME = GetValue(reader, "FIELD_NAME"),
                    MEMORY_ADDRESS = reader["MEMORY_ADDRESS"].ToString(),
                    SUBSCRIPTION = GetValue(reader, "SUBSCRIPTION"),
                    FIELD1 = reader["FIELD1"].ToString(),
                    FIELD2 = reader["FIELD2"].ToString(),
                    FIELD3 = reader["FIELD3"].ToString(),
                };
                lst.Add(config);
            }
            reader.Close();
            return lst;
        }
        /// <summary>
        /// 获取数据点配置集合
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public List<DC_DATA_CONFIG> GetDCDataConfigs(DC_DEVICE device)
        {
            return GetDCDataConfigs($" AND DEVICE_ID={device.ID}");
        }

        /// <summary>
        /// 获取数据点配置集合
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public List<DC_DATA_CONFIG> GetDCDataConfigs(DC_DEVICE device, int enable)
        {
            return GetDCDataConfigs($" AND DEVICE_ID={device.ID} AND ENABLE={enable}");
        }

        /// <summary>
        /// 获取数据点配置集合
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public List<DC_DATA_CONFIG> GetDCDataConfigs(List<int> ids, int enable)
        {
            return GetDCDataConfigs($" AND DEVICE_ID IN ({string.Join(",",ids)}) AND ENABLE={enable}");
        }

        public int InsertDataConfig(DC_DATA_CONFIG config)
        {
            string sql = $@"INSERT INTO DC.DC_DATA_CONFIG(
DEVICE_ID, 
NUM, 
NAME, 
MEMORY_ADDRESS, 
SUBSCRIPTION, 
DESCRIPTION, 
ENABLE,
TABLE_USER,
TABLE_NAME,
IDENTITY_COLUMN,
IDENTITY_VALUE,
FIELD_NAME,
FIELD_DATA_TYPE,
FIELD_DATA_LENGTH,
FIELD_DATA_PRECISION,
FIELD_DATA_SCALE,
CONVERTER
)VALUES(
{GetValueStr(config.DEVICE_ID)},
{GetValueStr(config.NUM)},
{GetValueStr(config.NAME)},
{GetValueStr(config.MEMORY_ADDRESS)},
{GetValueStr(config.SUBSCRIPTION)},
{GetValueStr(config.DESCRIPTION)},
{GetValueStr(config.ENABLE)},
{GetValueStr(config.TABLE_USER)},
{GetValueStr(config.TABLE_NAME)},
{GetValueStr(config.IDENTITY_COLUMN)},
{GetValueStr(config.IDENTITY_VALUE)},
{GetValueStr(config.FIELD_NAME)},
{GetValueStr(config.FIELD_DATA_TYPE)},
{GetValueStr(config.FIELD_DATA_LENGTH)},
{GetValueStr(config.FIELD_DATA_PRECISION)},
{GetValueStr(config.FIELD_DATA_SCALE)},
{GetValueStr(config.CONVERTER)}
) RETURNING ID INTO :ID";
            OracleParameter para = new OracleParameter("ID", OracleDbType.Int32);
            int res = OracleDataHelper.ExecuteNonQuery(sql, new OracleParameter[] { para });
            config.ID = ((OracleDecimal)para.Value).ToInt32();
            InsertRealData(config.ID);
            return res;
        }

        public int ModifyDataConfig(DC_DATA_CONFIG config)
        {
            string sql = $@"UPDATE DC.DC_DATA_CONFIG SET
DEVICE_ID={GetValueStr(config.DEVICE_ID)},
NUM={GetValueStr(config.NUM)},
NAME={GetValueStr(config.NAME)},
MEMORY_ADDRESS={GetValueStr(config.MEMORY_ADDRESS)},
SUBSCRIPTION={GetValueStr(config.SUBSCRIPTION)},
DESCRIPTION={GetValueStr(config.DESCRIPTION)},
ENABLE={GetValueStr(config.ENABLE)},
TABLE_USER={GetValueStr(config.TABLE_USER)},
TABLE_NAME={GetValueStr(config.TABLE_NAME)},
IDENTITY_COLUMN={GetValueStr(config.IDENTITY_COLUMN)},
IDENTITY_VALUE={GetValueStr(config.IDENTITY_VALUE)},
FIELD_NAME={GetValueStr(config.FIELD_NAME)},
FIELD_DATA_TYPE={GetValueStr(config.FIELD_DATA_TYPE)},
FIELD_DATA_LENGTH={GetValueStr(config.FIELD_DATA_LENGTH)},
FIELD_DATA_PRECISION={GetValueStr(config.FIELD_DATA_PRECISION)},
FIELD_DATA_SCALE={GetValueStr(config.FIELD_DATA_SCALE)},
CONVERTER={GetValueStr(config.CONVERTER)} WHERE ID='{config.ID}'
";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }

        public int DeleteDataConfig(DC_DATA_CONFIG config)
        {
            DeleteRealData(config.ID);
            string sql = $@"DELETE FROM DC.DC_DATA_CONFIG WHERE ID={GetValueStr(config.ID)}";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }
        #endregion


        /// <summary>
        /// 更新实时数据
        /// </summary>
        /// <param name="dataPointID"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        /// <param name="stamp"></param>
        /// <param name="updateTime"></param>
        /// <param name="updated"></param>
        /// <returns></returns>
        public int UpdateRealData(int dataID, object value, string quality, DateTime stamp, DateTime updateTime, int updated)
        {
            string sql = $@"UPDATE DC.DC_REAL_DATA SET VALUE='{value}',QUALITY='{quality}',
TIME_STAMP=TO_DATE('{stamp.ToString("yyyy/MM/dd HH:mm:ss")}','yyyy/mm/dd hh24:mi:ss'),
UPDATE_TIME=TO_DATE('{updateTime.ToString("yyyy/MM/dd HH:mm:ss")}','yyyy/mm/dd hh24:mi:ss'),
UPDATED={updated} WHERE DATA_CONFIG_ID={dataID}";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }

        #region DC_REAL_DATA
        /// <summary>
        /// 查询 DC_REAL_DATA
        /// </summary>
        /// <returns></returns>
        public List<DC_REAL_DATA> GetRealDatas()
        {
            string sql = $@"SELECT * FROM DC.DC_REAL_DATA WHERE UPDATED=1";
            OracleDataReader reader = OracleDataHelper.ExecuteReader(sql);
            List<DC_REAL_DATA> lst = new List<DC_REAL_DATA>();
            while (reader.Read())
            {
                DC_REAL_DATA data = new DC_REAL_DATA()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    DATA_CONFIG_ID = GetValue(reader, "DATA_CONFIG_ID"),
                    QUALITY = GetValue(reader, "QUALITY"),
                    VALUE = GetValue(reader, "VALUE"),
                    UPDATED = GetValue(reader, "UPDATED"),
                    TIME_STAMP = GetValue(reader, "TIME_STAMP"),
                    READ_TIME = GetValue(reader, "READ_TIME"),
                    UPDATE_TIME = GetValue(reader, "UPDATE_TIME"),
                    FIELD1 = reader["FIELD1"].ToString(),
                    FIELD2 = reader["FIELD2"].ToString(),
                    FIELD3 = reader["FIELD3"].ToString(),
                };
                lst.Add(data);
            }
            reader.Close();
            return lst;
        }

        /// <summary>
        /// 更新 DC_REAL_DATA 标志
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int UpdateRealDataFlag(List<int> ids)
        {
            string sql = $@"UPDATE DC.DC_REAL_DATA SET UPDATED=0 WHERE ID IN ({string.Join(",",ids)})";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }

        public int InsertRealData(int configID)
        {
            string sql = $@"INSERT INTO DC.DC_REAL_DATA(
DATA_CONFIG_ID, 
UPDATED)VALUES(
'{configID}',0)";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }

        public int DeleteRealData(int configID)
        {
            string sql = $"DELETE FROM DC.DC_REAL_DATA WHERE DATA_CONFIG_ID='{configID}'";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }
        #endregion

        #region 更新业务表
        /// <summary>
        /// 更新实时数据至业务表
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public int UpdateRealData2BusiTable(string user,string table,string identityValue, Dictionary<DC_DATA_CONFIG, DC_REAL_DATA> dic)
        {
            var datas = dic.Where(s => ValidateValueQuality(s.Value));//校验质量

            datas = ConvertValue(datas);//转换值

            datas = datas.Where(s => ValidateValueRange(s.Key, s.Value));//校验范围

            if(datas.Count()!=0)
            {
                string str = string.Join(",", datas.Select(s => $"{s.Key.FIELD_NAME}={GetValueStr(s.Key, s.Value)}"));//字段赋值字符串

                string sql = $@"UPDATE {user}.{table} SET {str} WHERE ID='{identityValue}'";//完整sql

                return OracleDataHelper.ExecuteNonQuery(sql);//执行
            }
            return 0;
        }

        /// <summary>
        /// 校验质量
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool ValidateValueQuality(DC_REAL_DATA data)
        {
            if (data.QUALITY.ToUpper() == "GOOD")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<DC_DATA_CONFIG, DC_REAL_DATA>> ConvertValue(IEnumerable<KeyValuePair<DC_DATA_CONFIG, DC_REAL_DATA>> datas)
        {
            Dictionary<DC_DATA_CONFIG, DC_REAL_DATA> objs = new Dictionary<DC_DATA_CONFIG, DC_REAL_DATA>();
            foreach (var item in datas)
            {
                string value = ParseValue(item.Value.VALUE, item.Key.FIELD_DATA_TYPE, item.Key.CONVERTER);
                item.Value.VALUE = value;
                objs.Add(item.Key, item.Value);
            }
            return objs;
        }

        /// <summary>
        /// 值转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataType"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        private string ParseValue(string value, string dataType, string converter)
        {
            string type = dataType.Trim().ToLower();
            if (type == "number")
            {
                if (!string.IsNullOrEmpty(converter))
                {
                    var exp = LambdaParser.Parse<Func<double, double>>(converter);
                    var func = exp.Compile();
                    double v = Convert.ToDouble(value);
                    return func(v).ToString();
                }
            }
            else if (type == "date")
            {
                string[] arr= value.Split(',');
                DateTime dt= ByteArray2DateTime(arr.Select(s=>Convert.ToByte(s)).ToArray());
                if (!string.IsNullOrEmpty(converter))
                {
                    var exp = LambdaParser.Parse<Func<DateTime, DateTime>>(converter);
                    var func = exp.Compile();
                    return func(dt).ToString();
                }
                return dt.ToString();
            }
            return value;
        }

        /// <summary>
        /// 校验范围
        /// </summary>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool ValidateValueRange(DC_DATA_CONFIG config, DC_REAL_DATA data)
        {
            string type = config.FIELD_DATA_TYPE.ToUpper();
            switch (type)
            {
                case "NUMBER":
                    double nValue = Convert.ToDouble(data.VALUE);
                    double max = Math.Pow(10, (double)(config.FIELD_DATA_PRECISION - config.FIELD_DATA_SCALE));
                    double min = -Math.Pow(10, (double)(config.FIELD_DATA_PRECISION - config.FIELD_DATA_SCALE));
                    if (nValue < max && nValue > min)
                    {
                        return true;
                    }
                    break;
                case "VARCHAR2":
                    string strValue = data.VALUE.ToString();
                    if (strValue.Length < config.FIELD_DATA_LENGTH + 1)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// 值的字符串
        /// </summary>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetValueStr(DC_DATA_CONFIG config, DC_REAL_DATA data)
        {
            if(string.IsNullOrEmpty(data.VALUE))
            {
                return "null";
            }
            string type = config.FIELD_DATA_TYPE.ToUpper();
            if (type == "DATE")
            {
                return $"TO_DATE('{data.VALUE}','yyyy/mm/dd hh24:mi:ss')";
            }
            return $"'{data.VALUE}'";
        }
        #endregion

        #region 天车动作
        /// <summary>
        /// 插入动作
        /// </summary>
        /// <param name="craID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="l_y"></param>
        /// <param name="z"></param>
        /// <param name="l_z"></param>
        /// <param name="wgt"></param>
        /// <param name="l_wgt"></param>
        /// <param name="symbol"></param>
        /// <param name="count"></param>
        /// <param name="cNum"></param>
        /// <returns></returns>
        public int InsertAction(int craID,object time,object x,object y,object l_y,object z,object l_z,object wgt,object l_wgt,object symbol,object count,object cNum)
        {
            string guid = Guid.NewGuid().ToString("N");
            string sql = $@"INSERT INTO PUB.CRANE_ACTION_RECORD(ID,CRA_ID,ACCEPT_TIME,OPERATION_TIME,COORD_X,COORD_Y,LIITLT_HOOK_Y,COORD_Z,LITTLE_HOOK_Z,
WEIGHT,LITTLE_HOOK_WEIGHT,ACTION_SYMBOL,HANGE_QUAN,CACHE_NUM)
VALUES('{guid}','{craID}',{GetDateTimeStr(DateTime.Now)},{GetDateTimeStr(time)},{GetValueStr(x)},{GetValueStr(y)},{GetValueStr(l_y)},{GetValueStr(z)},{GetValueStr(l_z)},
{GetValueStr(wgt)},{GetValueStr(l_wgt)},{GetValueStr(symbol)},{GetValueStr(count)},{GetValueStr(cNum)})";
            return OracleDataHelper.ExecuteNonQuery(sql);
        }


        #endregion

        #region 数据库表相关
        /// <summary>
        /// 查询数据库用户
        /// </summary>
        /// <returns></returns>
        public DataTable GetUsers()
        {
            string sql = "SELECT USERNAME FROM ALL_USERS WHERE ORACLE_MAINTAINED='N'";
            return OracleDataHelper.ExecuteDataTable(sql);
        }
        /// <summary>
        /// 查询数据库用户表单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetTables(string user)
        {
            string sql = $@"SELECT TABLE_NAME,OWNER FROM ALL_TABLES WHERE OWNER='{user}'";
            return OracleDataHelper.ExecuteDataTable(sql);
        }
        /// <summary>
        /// 查询id值
        /// </summary>
        /// <param name="user"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetIDs(string user,string table)
        {
            string sql = $@"SELECT ID FROM {user}.{table}";
            return OracleDataHelper.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 查询数据库表单列
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataTable GetColumns(string table)
        {
            string sql = $@" select OWNER, TABLE_NAME, COLUMN_NAME, DATA_TYPE, DATA_LENGTH, DATA_PRECISION, DATA_SCALE 
from all_tab_columns where Table_Name='{table}' ";
            return OracleDataHelper.ExecuteDataTable(sql);
        }

        #endregion

        #region CRANE

        public DataTable GetCranes()
        {
            string sql = "SELECT * FROM PUB.CRANE";
            return OracleDataHelper.ExecuteDataTable(sql);
        }

        public string GetCraRealInfoID(string craID)
        {
            string sql = $@"SELECT ID FROM PUB.CRANE_REAL_INFO WHERE CRA_ID='{craID}'";
            return OracleDataHelper.ExecuteScalar(sql).ToString();
        }
        #endregion

        #region 其他
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private dynamic GetValue(OracleDataReader reader, string propertyName)
        {
            if (reader[propertyName] == DBNull.Value)
            {
                return null;
            }
            return reader[propertyName];
        }

        private string GetDateTimeStr(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return "null";
            }
            return $"to_date('{obj}', 'yyyy/mm/dd hh24:mi:ss')";
        }

        private string GetValueStr(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return "null";
            }
            return $"'{obj}'";
        }

        /// <summary>
        /// 将字节数组转化为时间
        /// </summary>
        /// <param name="arrByte"></param>
        /// <returns></returns>
        private static DateTime ByteArray2DateTime(byte[] arrByte)
        {
            if (arrByte.Length == 6)
            {
                try
                {
                    string dt = "20" + arrByte[0].ToString("d2") + "-" + arrByte[1].ToString("d2") + "-" + arrByte[2].ToString("d2") + " " + arrByte[3].ToString("d2") + ":" + arrByte[4].ToString("d2") + ":" + arrByte[5].ToString("d2");
                    return Convert.ToDateTime(dt);
                }
                catch (Exception)
                {
                    throw new ArgumentException("字节数组格式错误，不能转化为时间");
                }
            }
            throw new ArgumentException("字节数组数量不等于6，不能转化为时间");
        }
        #endregion


    }
}
