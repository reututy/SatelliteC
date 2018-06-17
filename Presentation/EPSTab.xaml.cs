using DataModel.EPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
using Logic;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for EPSTab.xaml
    /// </summary>
    public partial class EPSTab : UserControl
    {
        private EPS eps;
        Timer myTimer;
        private GomEPS gomEPS;

        public EPSTab(GomEPS gomEPS)
        {
            InitializeComponent();
            this.gomEPS = gomEPS;
            gomEPS.GomEpsInitialize(0, 1);
            eps = gomEPS.eps_table[0];
            Battery batt = eps.OnboardBattery;
            BoostConverter converter1 = eps.BoostConverters[0];
            BoostConverter converter2 = eps.BoostConverters[1];
            BoostConverter converter3 = eps.BoostConverters[2];
            Channel channel1 = (Channel)eps.Outputs[3];
            Channel channel2 = (Channel)eps.Outputs[4];
            Channel channel3 = (Channel)eps.Outputs[5];
            Channel channel4 = (Channel)eps.Outputs[0];
            Channel channel5 = (Channel)eps.Outputs[1];
            Channel channel6 = (Channel)eps.Outputs[2];
            Output qs = eps.Outputs[6];
            Output qh = eps.Outputs[7];
            EPSConfiguration currConfig = eps.CurrentConfig;
            BatteryHeater heater = eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER];
            WDT i2c = eps.Wdts[(int)WdtType.I2C];
            WDT gnd = eps.Wdts[(int)WdtType.GND];
            WDT csp0 = eps.Wdts[(int)WdtType.CSP0];
            WDT csp1 = eps.Wdts[(int)WdtType.CSP1];
            //Battery
            batteryGrid.DataContext = batt;
            battCurrInText.DataContext = batt;
            battCurrOutText.DataContext = batt;
            //pv
            pv1Grid.DataContext = converter1;
            pv1CurrInText.DataContext = converter1;
            pv1CurrOutText.DataContext = converter1;
            pv2Grid.DataContext = converter2;
            pv2CurrInText.DataContext = converter2;
            pv2CurrOutText.DataContext = converter2;
            pv3Grid.DataContext = converter3;
            pv3CurrInText.DataContext = converter3;
            pv3CurrOutText.DataContext = converter3;
            //channels
            channel1Grid.DataContext = channel1;
            channel2Grid.DataContext = channel2;
            channel3Grid.DataContext = channel3;
            channel4Grid.DataContext = channel4;
            channel5Grid.DataContext = channel5;
            channel6Grid.DataContext = channel6;
            channelQSGrid.DataContext = qs;
            channelQHGrid.DataContext = qh;
            //current config
            confGrid.DataContext = currConfig;
            //battery heater
            batteryHeaterGrid.DataContext = heater;
            heaterModeText.DataContext = currConfig;
            //WDTs
            wdtI2CGrid.DataContext = i2c;
            wdtGNDGrid.DataContext = gnd;
            wdtCSP0Grid.DataContext = csp0;
            wdtCSP1Grid.DataContext = csp1;

            /*Task.Factory.StartNew(() => Thread.Sleep(1000))
            .ContinueWith((t) =>
            {
                eps.BatteryDrop();
            }, TaskScheduler.FromCurrentSynchronizationContext());
            */
            // Create a timer
            myTimer = new System.Timers.Timer();
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
            eps.RunEPS();
        }

        private void ChargeButton_Click(object sender, RoutedEventArgs e)
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

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (pauseButton.Content.Equals("Pause"))
            {
                pauseButton.Content = "Continue";
                pauseButton.Background = new SolidColorBrush(Colors.Blue);
                myTimer.Stop();
            }
            else
            {
                pauseButton.Content = "Pause";
                pauseButton.Background = new SolidColorBrush(Colors.Orange);
                myTimer.Start();
            }
        }
    }
}
