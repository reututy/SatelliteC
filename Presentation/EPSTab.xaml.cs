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
            Channel channel1 = (Channel)eps.Outputs[(int)OutputType.T_3_3V1];
            Channel channel2 = (Channel)eps.Outputs[(int)OutputType.T_3_3V2];
            Channel channel3 = (Channel)eps.Outputs[(int)OutputType.T_3_3V3];
            Channel channel4 = (Channel)eps.Outputs[(int)OutputType.T_5V1];
            Channel channel5 = (Channel)eps.Outputs[(int)OutputType.T_5V2];
            Channel channel6 = (Channel)eps.Outputs[(int)OutputType.T_5V3];
            Output qs = eps.Outputs[(int)OutputType.T_QS];
            Output qh = eps.Outputs[(int)OutputType.T_QH];
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
            pv1VboostText.DataContext = currConfig;
            pv2Grid.DataContext = converter2;
            pv2CurrInText.DataContext = converter2;
            pv2CurrOutText.DataContext = converter2;
            pv2VboostText.DataContext = currConfig;
            pv3Grid.DataContext = converter3;
            pv3CurrInText.DataContext = converter3;
            pv3CurrOutText.DataContext = converter3;
            pv3VboostText.DataContext = currConfig;
            //channels
            channel1Grid.DataContext = channel1;
            ch1OnDelText.DataContext = currConfig;
            ch1OffDelText.DataContext = currConfig;
            ch1CurrOutText.DataContext = channel1;

            channel2Grid.DataContext = channel2;
            ch2OnDelText.DataContext = currConfig;
            ch2OffDelText.DataContext = currConfig;
            ch2CurrOutText.DataContext = channel2;

            channel3Grid.DataContext = channel3;
            ch3OnDelText.DataContext = currConfig;
            ch3OffDelText.DataContext = currConfig;
            ch3CurrOutText.DataContext = channel3;

            channel4Grid.DataContext = channel4;
            ch4OnDelText.DataContext = currConfig;
            ch4OffDelText.DataContext = currConfig;
            ch4CurrOutText.DataContext = channel4;

            channel5Grid.DataContext = channel5;
            ch5OnDelText.DataContext = currConfig;
            ch5OffDelText.DataContext = currConfig;
            ch5CurrOutText.DataContext = channel5;

            channel6Grid.DataContext = channel6;
            ch6OnDelText.DataContext = currConfig;
            ch6OffDelText.DataContext = currConfig;
            ch6CurrOutText.DataContext = channel6;

            channelQSGrid.DataContext = qs;
            qsOnDelText.DataContext = currConfig;
            qsOffDelText.DataContext = currConfig;

            channelQHGrid.DataContext = qh;
            qhOnDelText.DataContext = currConfig;
            qhOffDelText.DataContext = currConfig;
            //current config
            confGrid.DataContext = currConfig;
            //battery heater
            batteryHeaterGrid.DataContext = heater;
            heaterModeText.DataContext = currConfig;
            heatLowText.DataContext = currConfig;
            heatHighText.DataContext = currConfig;
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
            //eps.IsCharging = !eps.IsCharging;
            if (chargeDischargeButton.Content.Equals("Charge"))
            {
                chargeDischargeButton.Content = "Discharge";
                chargeDischargeButton.Background = new SolidColorBrush(Colors.Red);

                panel1Button.Content = "OFF";
                sunmoon1Pic.Source = new BitmapImage(new Uri("Images/sun.png", UriKind.Relative));
                eps.BoostConverters[0].IsSun = true;

                panel2Button.Content = "OFF";
                sunmoon2Pic.Source = new BitmapImage(new Uri("Images/sun.png", UriKind.Relative));
                eps.BoostConverters[1].IsSun = true;

                panel3Button.Content = "OFF";
                sunmoon3Pic.Source = new BitmapImage(new Uri("Images/sun.png", UriKind.Relative));
                eps.BoostConverters[2].IsSun = true;
            }
            else
            {
                chargeDischargeButton.Content = "Charge";
                chargeDischargeButton.Background = new SolidColorBrush(Colors.Green);

                panel1Button.Content = "ON";
                sunmoon1Pic.Source = new BitmapImage(new Uri("Images/moon.png", UriKind.Relative));
                eps.BoostConverters[0].IsSun = false;

                panel2Button.Content = "ON";
                sunmoon2Pic.Source = new BitmapImage(new Uri("Images/moon.png", UriKind.Relative));
                eps.BoostConverters[1].IsSun = false;

                panel3Button.Content = "ON";
                sunmoon3Pic.Source = new BitmapImage(new Uri("Images/moon.png", UriKind.Relative));
                eps.BoostConverters[2].IsSun = false;
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

        private void Panel1Button_Click(object sender, RoutedEventArgs e)
        {
            if (panel1Button.Content.Equals("ON"))
            {
                panel1Button.Content = "OFF";
                sunmoon1Pic.Source = new BitmapImage(new Uri("Images/sun.png", UriKind.Relative));
                eps.BoostConverters[0].IsSun = true;
            }
            else
            {
                panel1Button.Content = "ON";
                sunmoon1Pic.Source = new BitmapImage(new Uri("Images/moon.png", UriKind.Relative));
                eps.BoostConverters[0].IsSun = false;
            }
        }

        private void Panel2Button_Click(object sender, RoutedEventArgs e)
        {
            if (panel2Button.Content.Equals("ON"))
            {
                panel2Button.Content = "OFF";
                sunmoon2Pic.Source = new BitmapImage(new Uri("Images/sun.png", UriKind.Relative));
                eps.BoostConverters[1].IsSun = true;
            }
            else
            {
                panel2Button.Content = "ON";
                sunmoon2Pic.Source = new BitmapImage(new Uri("Images/moon.png", UriKind.Relative));
                eps.BoostConverters[1].IsSun = false;
            }
        }

        private void Panel3Button_Click(object sender, RoutedEventArgs e)
        {
            if (panel3Button.Content.Equals("ON"))
            {
                panel3Button.Content = "OFF";
                sunmoon3Pic.Source = new BitmapImage(new Uri("Images/sun.png", UriKind.Relative));
                eps.BoostConverters[2].IsSun = true;
            }
            else
            {
                panel3Button.Content = "ON";
                sunmoon3Pic.Source = new BitmapImage(new Uri("Images/moon.png", UriKind.Relative));
                eps.BoostConverters[2].IsSun = false;
            }
        }

        private void BattHeaterButton_Click(object sender, RoutedEventArgs e)
        {
            if (battHeaterButton.Content.Equals("ON"))
            {
                battHeaterButton.Content = "OFF";
                eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status = EPSConstants.ON;
            }
            else
            {
                battHeaterButton.Content = "ON";
                eps.BatteryHeaters[EPSConstants.ONBOARD_HEATER].Status = EPSConstants.OFF;
            }
        }

        private void Ch1Button_Click(object sender, RoutedEventArgs e)
        {
            if (ch1Button.Content.Equals("ON"))
            {
                ch1Button.Content = "OFF";
                eps.Outputs[(int)OutputType.T_3_3V1].Status = EPSConstants.ON;
            }
            else
            {
                ch1Button.Content = "ON";
                eps.Outputs[(int)OutputType.T_3_3V1].Status = EPSConstants.OFF;
            }
            
        }

        private void Ch2Button_Click(object sender, RoutedEventArgs e)
        {
            if (ch2Button.Content.Equals("ON"))
            {
                ch2Button.Content = "OFF";
                eps.Outputs[(int)OutputType.T_3_3V2].Status = EPSConstants.ON;
            }
            else
            {
                ch2Button.Content = "ON";
                eps.Outputs[(int)OutputType.T_3_3V2].Status = EPSConstants.OFF;
            }
        }
        private void Ch3Button_Click(object sender, RoutedEventArgs e)
        {
            if (ch3Button.Content.Equals("ON"))
            {
                ch3Button.Content = "OFF";
                eps.Outputs[(int)OutputType.T_3_3V3].Status = EPSConstants.ON;
            }
            else
            {
                ch3Button.Content = "ON";
                eps.Outputs[(int)OutputType.T_3_3V3].Status = EPSConstants.OFF;
            }
        }
        private void Ch4Button_Click(object sender, RoutedEventArgs e)
        {
            if (ch4Button.Content.Equals("ON"))
            {
                ch4Button.Content = "OFF";
                eps.Outputs[(int)OutputType.T_5V1].Status = EPSConstants.ON;
            }
            else
            {
                ch4Button.Content = "ON";
                eps.Outputs[(int)OutputType.T_5V1].Status = EPSConstants.OFF;
            }
        }
        private void Ch5Button_Click(object sender, RoutedEventArgs e)
        {
            if (ch5Button.Content.Equals("ON"))
            {
                ch5Button.Content = "OFF";
                eps.Outputs[(int)OutputType.T_5V2].Status = EPSConstants.ON;
            }
            else
            {
                ch5Button.Content = "ON";
                eps.Outputs[(int)OutputType.T_5V2].Status = EPSConstants.OFF;
            }
        }

        private void Ch6Button_Click(object sender, RoutedEventArgs e)
        {
            if (ch6Button.Content.Equals("ON"))
            {
                ch6Button.Content = "OFF";
                eps.Outputs[(int)OutputType.T_5V3].Status = EPSConstants.ON;
            }
            else
            {
                ch6Button.Content = "ON";
                eps.Outputs[(int)OutputType.T_5V3].Status = EPSConstants.OFF;
            }
        }

        private void QsButton_Click(object sender, RoutedEventArgs e)
        {
            if (qsButton.Content.Equals("ON"))
            {
                qsButton.Content = "OFF";
                eps.Outputs[(int)OutputType.T_QS].Status = EPSConstants.ON;
            }
            else
            {
                qsButton.Content = "ON";
                eps.Outputs[(int)OutputType.T_QS].Status = EPSConstants.OFF;
            }
        }

        private void QhButton_Click(object sender, RoutedEventArgs e)
        {
            if (qhButton.Content.Equals("ON"))
            {
                qhButton.Content = "OFF";
                eps.Outputs[(int)OutputType.T_QH].Status = EPSConstants.ON;
            }
            else
            {
                qhButton.Content = "ON";
                eps.Outputs[(int)OutputType.T_QH].Status = EPSConstants.OFF;
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            string path = extract_file.Text;
            String[] toWrite = GomEPS.logs.ToArray<String>();
            try
            {
                System.IO.File.WriteAllLines(path, toWrite);
            }
            catch (Exception)
            {
                MessageBox.Show("Illegal file path");
            }
        }
    }

}
