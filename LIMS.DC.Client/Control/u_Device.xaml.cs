using LIMS.DC.DAL;
using LIMS.DC.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIMS.DC.Client.Control
{
    /// <summary>
    /// Interaction logic for u_Server.xaml
    /// </summary>
    public partial class u_Device : UserControl
    {
        public u_Device(DC_DEVICE device)
        {
            Device = device;
            DC_Service dC_Service = new DC_Service();
            try
            {
                Cranes = dC_Service.GetCranes();
            }
            catch (Exception)
            {

            }
            InitializeComponent();
        }

        public DC_DEVICE Device { get; set; }

        public DataTable Cranes { get; set; }

    }
}
