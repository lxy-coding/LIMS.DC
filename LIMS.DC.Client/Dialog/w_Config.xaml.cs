using LIMS.DC.DAL;
using LIMS.DC.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class w_Config : MetroWindow, INotifyPropertyChanged
    {
        public w_Config(DC_DEVICE device)
        {
            Config.DEVICE_ID = device.ID;

            InitializeComponent();
        }

        public w_Config(DC_DATA_CONFIG config)
        {
            IsModify = true;
            Config = config;
            InitializeComponent();
        }
        private bool IsModify = false;

        public DataTable Users { get; set; }

        public DataTable Tables { get; set; }

        public DataTable IDs { get; set; }

        public DataTable Columns { get; set; }

        /// <summary>
        /// 属性变化事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 触发属性变化事件
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        DC_Service dC_Service = new DC_Service();

        public DC_DATA_CONFIG Config { get; set; } = new DC_DATA_CONFIG();


        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Users = dC_Service.GetUsers();
                OnPropertyChanged("Users");
            }
            catch (Exception)
            {

            }
        }


        private void User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(Config.TABLE_USER))
                {
                    Tables = null;
                }
                else
                {
                    Tables = dC_Service.GetTables(Config.TABLE_USER);
                }
                OnPropertyChanged("Tables");
            }
            catch (Exception)
            {

            }
        }


        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(Config.TABLE_USER) || string.IsNullOrEmpty(Config.TABLE_NAME))
                {
                    IDs = null;
                    Columns = null;
                }
                else
                {
                    IDs = dC_Service.GetIDs(Config.TABLE_USER, Config.TABLE_NAME);
                    Columns = dC_Service.GetColumns(Config.TABLE_NAME);
                }
                OnPropertyChanged("IDs");
                OnPropertyChanged("Columns");
            }
            catch (Exception)
            {

            }
        }


        private void Column_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            DataRowView drv = cmb.SelectedItem as DataRowView;
            if(drv!=null)
            {
                Config.FIELD_DATA_TYPE = drv["DATA_TYPE"].ToString();
                Config.FIELD_DATA_LENGTH = GetValue( drv["DATA_LENGTH"]);
                Config.FIELD_DATA_PRECISION = GetValue(drv["DATA_PRECISION"]);
                Config.FIELD_DATA_SCALE = GetValue(drv["DATA_SCALE"]);
            }
            else
            {
                Config.FIELD_DATA_TYPE = null;
                Config.FIELD_DATA_LENGTH = null;
                Config.FIELD_DATA_PRECISION = null;
                Config.FIELD_DATA_SCALE = null;
            }
            OnPropertyChanged("Config");
        }

        private int? GetValue(object obj)
        {
            if(obj is DBNull || obj is null)
            {
                return null;
            }
            return Convert.ToInt32(obj);
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Config.NUM))
            {
                MessageBox.Show("编号不能为空。");
                return;
            }
            try
            {
                if (IsModify)
                {
                    dC_Service.ModifyDataConfig(Config);
                }
                else
                {
                    dC_Service.InsertDataConfig(Config);
                    dC_Service.InsertRealData(Config.ID);
                }
                MessageBox.Show("操作成功。");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败。" + ex.Message);
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
