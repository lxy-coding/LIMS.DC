using LIMS.DC.DAL;
using LIMS.DC.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LIMS.DC.Client.Dialog
{
    /// <summary>
    /// Interaction logic for w_Server.xaml
    /// </summary>
    public partial class w_AddCraneConfig : MetroWindow
    {
        public w_AddCraneConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 数据访问对象
        /// </summary>
        DC_Service dC_Service = new DC_Service();

        public Action<DC_SERVER, DC_DEVICE> AddCraSuccess;

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DC_SERVER server = this.cmb_Server.SelectedItem as DC_SERVER;
                DataRowView cra = this.cmb_Crane.SelectedItem as DataRowView;

                DC_DEVICE device = new DC_DEVICE()
                {
                    CHANNEL = "S7",
                    CRA_ID = Convert.ToInt32(cra["ID"]),
                    ENABLE = 1,
                    NAME = cra["CRA_NAME"].ToString(),
                    NUM = cra["CRA_NUM"].ToString(),
                    S7CONNECTION = this.txt_Conn.Text.Trim(),
                    DESCRIPTION = cra["CRA_NAME"].ToString(),
                    SERVER_ID = server.ID,
                    Configs = new ObservableCollection<DC_DATA_CONFIG>(),
                };
                dC_Service.InsertDCDevice(device);

                DataTable columnInfo = dC_Service.GetColumns("CRANE_REAL_INFO");

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "COORD_X");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_X",
                        NAME = "实时_X",
                        MEMORY_ADDRESS = "DB1,DINT4000",
                        DESCRIPTION = "实时_X",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "COORD_Y");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_Y",
                        NAME = "实时_Y",
                        MEMORY_ADDRESS = "DB1,DINT4004",
                        DESCRIPTION = "实时_Y",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "LITTLE_HOOK_Y");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_LITTLE_Y",
                        NAME = "实时_副Y",
                        MEMORY_ADDRESS = "DB1,DINT4008",
                        DESCRIPTION = "实时_副Y",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "COORD_Z");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_Z",
                        NAME = "实时_Z",
                        MEMORY_ADDRESS = "DB1,DINT4012",
                        DESCRIPTION = "实时_Z",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "LITTLE_HOOK_Z");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_LITTLE_Z",
                        NAME = "实时_副Z",
                        MEMORY_ADDRESS = "DB1,DINT4016",
                        DESCRIPTION = "实时_副Z",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "WEIGHT");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_WEIGHT",
                        NAME = "实时_主钩重量",
                        MEMORY_ADDRESS = "DB1,DINT4020",
                        DESCRIPTION = "实时_主钩重量",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "LITTLE_HOOK_WEIGHT");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_LITTLE_WEIGHT",
                        NAME = "实时_副钩重量",
                        MEMORY_ADDRESS = "DB1,DINT4024",
                        DESCRIPTION = "实时_副钩重量",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "OBJ_COUNT");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_OBJ_COUNT",
                        NAME = "实时_吊物数量",
                        MEMORY_ADDRESS = "DB1,B4029",
                        DESCRIPTION = "实时_吊物数量",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DataRow row = columnInfo.Rows.Cast<DataRow>().First(s => s["COLUMN_NAME"].ToString() == "IS_ONLINE");
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "REAL_IS_ONLINE",
                        NAME = "实时_网络状态",
                        MEMORY_ADDRESS = "&statepathval()",
                        DESCRIPTION = "实时_网络状态",
                        SUBSCRIPTION = 1,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                        IDENTITY_VALUE = dC_Service.GetCraRealInfoID(cra["ID"].ToString()),
                        TABLE_USER = "PUB",
                        TABLE_NAME = "CRANE_REAL_INFO",
                        FIELD_NAME = row["COLUMN_NAME"].ToString(),
                        FIELD_DATA_TYPE = row["DATA_TYPE"].ToString(),
                        FIELD_DATA_LENGTH = GetValue(row["DATA_LENGTH"]),
                        FIELD_DATA_PRECISION = GetValue(row["DATA_PRECISION"]),
                        FIELD_DATA_SCALE = GetValue(row["DATA_SCALE"]),
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_X",
                        NAME = "动作_X",
                        MEMORY_ADDRESS = "DB1,DINT4054",
                        DESCRIPTION = "动作_X",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_Y",
                        NAME = "动作_Y",
                        MEMORY_ADDRESS = "DB1,DINT4058",
                        DESCRIPTION = "动作_Y",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_LITTLE_Y",
                        NAME = "动作_副Y",
                        MEMORY_ADDRESS = "DB1,DINT4062",
                        DESCRIPTION = "动作_副Y",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_Z",
                        NAME = "动作_Z",
                        MEMORY_ADDRESS = "DB1,DINT4066",
                        DESCRIPTION = "动作_Z",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_LITTLE_Z",
                        NAME = "动作_副Z",
                        MEMORY_ADDRESS = "DB1,DINT4070",
                        DESCRIPTION = "动作_副Z",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_WEIGHT",
                        NAME = "动作_主钩重量",
                        MEMORY_ADDRESS = "DB1,DINT4074",
                        DESCRIPTION = "动作_主钩重量",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_LITTLE_WEIGHT",
                        NAME = "动作_副钩重量",
                        MEMORY_ADDRESS = "DB1,DINT4078",
                        DESCRIPTION = "动作_副钩重量",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_SYMBOL",
                        NAME = "动作_动作类型",
                        MEMORY_ADDRESS = "DB1,B4082",
                        DESCRIPTION = "动作_动作类型",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_OBJ_COUNT",
                        NAME = "动作_吊物数量",
                        MEMORY_ADDRESS = "DB1,B4083",
                        DESCRIPTION = "动作_吊物数量",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_CACHE_NUM",
                        NAME = "动作_缓存序号",
                        MEMORY_ADDRESS = "DB1,DINT4050",
                        DESCRIPTION = "动作_缓存序号",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_READED_NUM",
                        NAME = "动作_已读序号",
                        MEMORY_ADDRESS = "DB1,DINT4035",
                        DESCRIPTION = "动作_已读序号",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "ACTION_TIME",
                        NAME = "动作_动作时间",
                        MEMORY_ADDRESS = "DB1,B4084,6",
                        DESCRIPTION = "动作_动作时间",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "CAL_TIME",
                        NAME = "校正_校正时间",
                        MEMORY_ADDRESS = "DB1,B4100,6",
                        DESCRIPTION = "校正_校正时间",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                {
                    DC_DATA_CONFIG config = new DC_DATA_CONFIG()
                    {
                        NUM = "CAL_WRITE_FLAG",
                        NAME = "校正_写入标志",
                        MEMORY_ADDRESS = "DB1,B4106",
                        DESCRIPTION = "校正_写入标志",
                        SUBSCRIPTION = 0,
                        DEVICE_ID = device.ID,
                        ENABLE = 1,
                    };
                    dC_Service.InsertDataConfig(config);
                    device.Configs.Add(config);
                }

                MessageBox.Show("操作成功。");

                AddCraSuccess?.Invoke(server,device);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int? GetValue(object obj)
        {
            if (obj is DBNull || obj is null)
            {
                return null;
            }
            return Convert.ToInt32(obj);
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.cmb_Server.ItemsSource = dC_Service.GetDCServers();
                this.cmb_Crane.ItemsSource = dC_Service.GetCranes().DefaultView;
            }
            catch (Exception)
            {

            }
        }
    }
}
