using LIMS.DC.Client.Dialog;
using LIMS.DC.DAL;
using LIMS.DC.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIMS.DC.Client.Control
{
    /// <summary>
    /// u_DataConfig.xaml 的交互逻辑
    /// </summary>
    public partial class u_DataConfig : UserControl, INotifyPropertyChanged
    {
        public u_DataConfig()
        {
            InitializeComponent();
        }

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

        /// <summary>
        /// 数据访问对象
        /// </summary>
        DC_Service dC_Service = new DC_Service();

        /// <summary>
        /// 数据源
        /// </summary>
        public ObservableCollection<DC_SERVER> Servers { get; set; }

        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Load()
        {
            try
            {
                List<DC_SERVER> servers = dC_Service.GetDCServers().OrderBy(s => s.NAME).ToList();
                foreach (var server in servers)
                {
                    server.Devices = new ObservableCollection<DC_DEVICE>(dC_Service.GetDCDevices(server).OrderBy(s => s.NAME).ToList());
                    foreach (var device in server.Devices)
                    {
                        device.Configs = new ObservableCollection<DC_DATA_CONFIG>(dC_Service.GetDCDataConfigs(device).OrderBy(s => s.NAME).ToList());
                    }
                }
                Servers = new ObservableCollection<DC_SERVER>(servers);
                OnPropertyChanged("Servers");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载异常。" + ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tv_Main_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is DC_SERVER)
            {
                Detail = new u_Server((DC_SERVER)e.NewValue);
            }
            else if (e.NewValue is DC_DEVICE)
            {
                Detail = new u_Device((DC_DEVICE)e.NewValue);
            }
            else if (e.NewValue is DC_DATA_CONFIG)
            {
                Detail = new u_Config((DC_DATA_CONFIG)e.NewValue);
            }
            OnPropertyChanged("Detail");
        }

        public UserControl Detail { get; set; }
        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Server(object sender, RoutedEventArgs e)
        {
            w_Server window = new w_Server();
            if (window.ShowDialog() == true)
            {
                Servers.Add(window.Server);
            }
        }
        /// <summary>
        /// 编辑服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Modify_Server(object sender, RoutedEventArgs e)
        {
            DC_SERVER server = (DC_SERVER)(sender as FrameworkElement).Tag;
            w_Server window = new w_Server(server);
            if (window.ShowDialog() != true)
            {
                Load();
            }
        }
        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Del_Server(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DC_SERVER server = (DC_SERVER)(sender as FrameworkElement).Tag;
                try
                {
                    dC_Service.DeleteDCServer(server);
                    MessageBox.Show("删除成功。");
                    Servers.Remove(server);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除异常。"+ex.Message);
                }
            }
        }
        /// <summary>
        /// 添加plc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Add_Device(object sender, RoutedEventArgs e)
        {
            DC_SERVER server = (DC_SERVER)(sender as FrameworkElement).Tag;
            w_Device window = new w_Device(server);
            if (window.ShowDialog() == true)
            {
                server.Devices.Add(window.Device);
            }
        }
        /// <summary>
        /// 编辑plc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Modify_Device(object sender, RoutedEventArgs e)
        {
            DC_DEVICE device = (DC_DEVICE)(sender as FrameworkElement).Tag;
            w_Device window = new w_Device(device);
            if (window.ShowDialog() != true)
            {
                Load();
            }
        }
        /// <summary>
        /// 删除plc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Del_Device(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DC_DEVICE device = (DC_DEVICE)(sender as FrameworkElement).Tag;
                try
                {
                    dC_Service.DeleteDevice(device);
                    MessageBox.Show("删除成功。");
                    DC_SERVER server = Servers.First(s=>s.Devices.Contains(device));
                    server.Devices.Remove(device);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除异常。" + ex.Message);
                }
            }
        }
        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Add_Config(object sender, RoutedEventArgs e)
        {
            DC_DEVICE device = (DC_DEVICE)(sender as FrameworkElement).Tag;
            w_Config window = new w_Config(device);
            if (window.ShowDialog() == true)
            {
                device.Configs.Add(window.Config);
            }
        }
        /// <summary>
        /// 编辑项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Modify_Config(object sender, RoutedEventArgs e)
        {
            DC_DATA_CONFIG config = (DC_DATA_CONFIG)(sender as FrameworkElement).Tag;
            w_Config window = new w_Config(config);
            if (window.ShowDialog() != true)
            {
                Load();
            }
        }
        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_Del_Config(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DC_DATA_CONFIG config = (DC_DATA_CONFIG)(sender as FrameworkElement).Tag;
                try
                {
                    dC_Service.DeleteDataConfig(config);
                    dC_Service.DeleteRealData(config.ID);
                    MessageBox.Show("删除成功。");
                    DC_SERVER server = Servers.First(s => s.Devices.FirstOrDefault(p=>p.ID==config.DEVICE_ID)!=null);
                    DC_DEVICE device = server.Devices.First(s=>s.ID==config.DEVICE_ID);
                    device.Configs.Remove(config);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除异常。" + ex.Message);
                }
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reflesh(object sender, RoutedEventArgs e)
        {
            Load();
        }
        /// <summary>
        /// 添加天车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Crane(object sender, RoutedEventArgs e)
        {
            w_AddCraneConfig window = new w_AddCraneConfig();
            window.AddCraSuccess += (server, device) => {
                DC_SERVER sERVER= Servers.First(s=>s.ID==server.ID);
                sERVER.Devices.Add(device);
            };
            window.Show();
        }
    }
}
