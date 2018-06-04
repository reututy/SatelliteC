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

namespace Presentation
{
    /// <summary>
    /// Interaction logic for ComponentsTabs.xaml
    /// </summary>
    public partial class ComponentsTabs : UserControl
    {
        public ComponentsTabs(Logic.GomEPS eps, Logic.IsisTRXVU trx)
        {
            InitializeComponent();
            TRXTab tRXTab = new TRXTab(trx);
            trxpanel.Children.Add(tRXTab);
            tRXTab.Visibility = Visibility.Visible;

            EPSTab ePXTab = new EPSTab(eps);
            epspanel.Children.Add(ePXTab);
            ePXTab.Visibility = Visibility.Visible;

            OBCTab oBCTab = new OBCTab();
            obcpanel.Children.Add(oBCTab);
            oBCTab.Visibility = Visibility.Visible;

        }
    }
}
