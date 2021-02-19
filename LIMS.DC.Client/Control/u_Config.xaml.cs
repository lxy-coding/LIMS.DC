using LIMS.DC.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIMS.DC.Client.Control
{
    /// <summary>
    /// Interaction logic for u_Server.xaml
    /// </summary>
    public partial class u_Config : UserControl
    {
        public u_Config(DC_DATA_CONFIG config)
        {
            Config = config;
            InitializeComponent();
        }

        public DC_DATA_CONFIG Config { get; set; }
    }
}
