using LIMS.DC.DAL;
using LIMS.DC.Model;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    public partial class w_Server : MetroWindow
    {
        public w_Server()
        {
            InitializeComponent();
        }

        public w_Server(DC_SERVER server)
        {
            //Server.ID = server.ID;
            //Server.IP_ADDRESS = server.IP_ADDRESS;
            //Server.NAME = server.NAME;
            //Server.NUM = server.NUM;
            //Server.ENABLE = server.ENABLE;
            //Server.FIELD1 = server.FIELD1;
            //Server.FIELD2 = server.FIELD2;
            //Server.FIELD3 = server.FIELD3;
            Server = server;
            IsModify = true;
            InitializeComponent();
        }

        private bool IsModify = false;

        public DC_SERVER Server { get; set; } = new DC_SERVER();

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Server.NUM))
            {
                MessageBox.Show("编号不能为空。");
                return;
            }
            DC_Service dC_Service = new DC_Service();
            try
            {
                if(IsModify)
                {
                    dC_Service.ModifyDCServer(Server);
                }
                else
                {
                    dC_Service.InsertDCServer(Server);
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
