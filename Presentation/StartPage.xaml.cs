using DataModel.EPS;
using DataModel.TRX;
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

        private byte trx_number = Convert.ToByte(1);
        private ISIStrxvuFrameLengths trxLens = new ISIStrxvuFrameLengths();
        private ISIStrxvuBitrate trxBitrate = ISIStrxvuBitrate.trxvu_bitrate_1200;

        public StartPage()
        {
            trxLens.maxAX25frameLengthRX = 2048;
            trxLens.maxAX25frameLengthTX = 2048;
            InitializeComponent();
            InitEPSVals();
            bitRateSelect.ItemsSource = Enum.GetValues(typeof(ISIStrxvuBitrate)).Cast<ISIStrxvuBitrate>();
            bitRateSelect.SelectedValue = ISIStrxvuBitrate.trxvu_bitrate_1200;
            heater_mode.ItemsSource = Enum.GetValues(typeof(HeaterMode)).Cast<HeaterMode>();
            pptModeText.ItemsSource = Enum.GetValues(typeof(PPTMode)).Cast<PPTMode>();
            


            pvTempText.Text = EPSStartValues.PVTemp.ToString();
            pptModeText.SelectedValue = EPSStartValues.PPTmode;
            batt_temp.Text = EPSStartValues.BattTemp.ToString();
            batt_heat_low.Text = EPSStartValues.BattHeaterLow.ToString();
            batt_heat_high.Text = EPSStartValues.BattHeaterHigh.ToString();
            heater_mode.SelectedValue = HeaterMode.AUTO;

        }

        private void trxInit(Logic.IsisTRXVU trx)
        {
            ISIStrxvuI2CAddress[] trx_add = new ISIStrxvuI2CAddress[trx_number];
            ISIStrxvuFrameLengths[] trx_f_lens = new ISIStrxvuFrameLengths[trx_number];
            for (int i = 0; i < trx_add.Length; i++)
            {
                trx_add[i] = new ISIStrxvuI2CAddress();
                trx_f_lens[i] = new ISIStrxvuFrameLengths();
                trx_f_lens[i].maxAX25frameLengthRX = trxLens.maxAX25frameLengthRX;
                trx_f_lens[i].maxAX25frameLengthTX = trxLens.maxAX25frameLengthTX;
            }

            trx.IsisTrxvu_initialize(trx_add, trx_f_lens, trxBitrate, trx_number);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logic.GomEPS eps = new Logic.GomEPS();
            Logic.IsisTRXVU trx = new Logic.IsisTRXVU();
            trxInit(trx);
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
            string error = "";
            short res;
            if (Int16.TryParse(pvTempText.Text, out res))
            {
                EPSStartValues.PVTemp = res;
            }
            else
            {
                error = "Photovolatic converters should be a number";
            }
            if (pptModeText.SelectedValue != null)
            {
                EPSStartValues.PPTmode = (PPTMode)pptModeText.SelectedValue;
            }
            short batttemp;
            if (Int16.TryParse(batt_temp.Text, out batttemp))
            {
                EPSStartValues.BattTemp = batttemp;
            }
            else
            {
                error += "\n battery temperature should be a number";
            }

            byte battlow;
            if (byte.TryParse(batt_heat_low.Text, out battlow))
            {
                EPSStartValues.BattHeaterLow = battlow;
            }
            else
            {
                error += "\n battery low should be a number";
            }

            byte batthigh;
            if (byte.TryParse(batt_heat_high.Text, out batthigh))
            {
                EPSStartValues.BattHeaterHigh = batthigh;
            }
            else
            {
                error += "\n battery high should be a number";
            }
            if (heater_mode.SelectedValue != null)
            {
                EPSStartValues.BattHeaterMode = (HeaterMode)heater_mode.SelectedValue;
            }


            byte trx_num;
            if (byte.TryParse(num_of_trx.Text, out trx_num))
            {
                if (trx_num < 1)
                {
                    error += "\n trx units number should be greater then 0";
                }
                else
                {
                    trx_number = trx_num;
                }
                
            }
            else
            {
                error += "\n trx units number should be a number";
            }

            uint max_f_rx = 2048;
            if (uint.TryParse(max_rx.Text, out max_f_rx))
            {
                trxLens.maxAX25frameLengthRX = max_f_rx;
            }
            else
            {
                error += "\n max frame length should be a number";
            }

            uint max_f_tx = 2048;
            if (uint.TryParse(max_tx.Text, out max_f_tx))
            {
                trxLens.maxAX25frameLengthTX = max_f_tx;
            }
            else
            {
                error += "\n max frame length should be a number";
            }

            if (bitRateSelect.SelectedValue != null)
            {
                trxBitrate = (ISIStrxvuBitrate)bitRateSelect.SelectedValue;
            }

            if (!error.Equals(""))
            {
                MessageBox.Show(error);
            }
        }
    }
}
