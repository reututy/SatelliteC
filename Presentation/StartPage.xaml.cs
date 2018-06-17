using DataModel.EPS;
using DemoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
            InitEPSVals();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logic.GomEPS eps = new Logic.GomEPS();
            Logic.IsisTRXVU trx = new Logic.IsisTRXVU();
            Logic.FRAMLogic fram = new Logic.FRAMLogic();
            try
            {
                AsyncService service = new AsyncService();
                AsyncService.eps = eps;
                AsyncService.trx = trx;
                AsyncService.fram = fram;
                Thread newThread = new Thread(AsyncService.Run);
                newThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            MainWindow.ChangePanel(new ComponentsTabs(eps, trx));
            // number of trxes.. defaults..
        }

        private void InitEPSVals()
        {
            pvTempText.Text = Convert.ToString(EPSStartValues.PVTemp);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            short res;
            if (Int16.TryParse(pvTempText.Text, out res))
            {
                EPSStartValues.PVTemp = res;
            }
            else
            {
                //error
            }
        }
    }
}
