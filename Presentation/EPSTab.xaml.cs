using DataModel.EPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
//using System.Threading;
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
    /// Interaction logic for EPSTab.xaml
    /// </summary>
    public partial class EPSTab : UserControl
    {
        private EPS eps;

        public EPSTab()
        {
            InitializeComponent();
            eps = new EPS();
            currInText.DataContext = eps.onboard_battery;
            currOutText.DataContext = eps.onboard_battery;
            voltText.DataContext = eps.onboard_battery;
            tempText.DataContext = eps.onboard_battery;
            battStateText.DataContext = eps.onboard_battery;
            /*Task.Factory.StartNew(() => Thread.Sleep(1000))
            .ContinueWith((t) =>
            {
                eps.BatteryDrop();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            */
            // Create a timer
            Timer myTimer = new System.Timers.Timer();
            // Tell the timer what to do when it elapses
            myTimer.Elapsed += new ElapsedEventHandler(myEvent);
            // Set it to go off every five seconds
            myTimer.Interval = 1000;
            // And start it        
            myTimer.Enabled = true;

            // Implement a call with the right signature for events going off
            

        }
        private void myEvent(object source, ElapsedEventArgs e)
        {
            eps.BatteryDrop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eps.IsCharging = !eps.IsCharging;
            if (chargeDischargeButton.Content.Equals("Charge"))
            {
                chargeDischargeButton.Content = "Discharge";
                chargeDischargeButton.Background = new SolidColorBrush(Colors.Red);
            }
            else
            {
                chargeDischargeButton.Content = "Charge";
                chargeDischargeButton.Background = new SolidColorBrush(Colors.Green);
            }

            //eps.ChargingFlow();
        }

    }
}
