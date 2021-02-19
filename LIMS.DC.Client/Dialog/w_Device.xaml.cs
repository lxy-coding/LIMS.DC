using LIMS.DC.DAL;
using LIMS.DC.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    public partial class w_Device : MetroWindow
    {
        public w_Device(DC_SERVER server)
        {
            Device.SERVER_ID = server.ID;
            DC_Service dC_Service = new DC_Service();
            try
            {
                DataTable dt = dC_Service.GetCranes();
                DataRow row = dt.NewRow();
                row["CRA_NAME"] = "不是天车选这个";
                dt.Rows.InsertAt(row, 0);
                Cranes = dt;
            }
            catch (Exception)
            {

            }
            InitializeComponent();
        }

        public w_Device(DC_DEVICE device)
        {
            IsModify = true;
            Device = device;
            DC_Service dC_Service = new DC_Service();
            try
            {
                DataTable dt = dC_Service.GetCranes();
                DataRow row= dt.NewRow();
                row["CRA_NAME"] = "不是天车选这个";
                dt.Rows.InsertAt(row,0);
                Cranes = dt;
            }
            catch (Exception)
            {

            }
            InitializeComponent();
        }
        private bool IsModify = false;


        public DC_DEVICE Device { get; set; } = new DC_DEVICE();

        public DataTable Cranes{ get; set; }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Device.NUM))
            {
                MessageBox.Show("编号不能为空。");
                return;
            }
            DC_Service dC_Service = new DC_Service();
            try
            {
                if (IsModify)
                {
                    dC_Service.ModifyDevice(Device);
                }
                else
                {
                    dC_Service.InsertDCDevice(Device);
                }
                MessageBox.Show("操作成功。");
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败。"+ex.Message);
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
