using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using DataModel;
using DataModel.TRX;
using Logic;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for TRX.xaml
    /// </summary>
    public partial class TRXTab : UserControl
    {
        private Array trxesList;
        private TestThreadGui testt;
        private IsisTRXVU isisTRXVU;
        private AX25Frame currFrame;

        private void initiallizeTRX()
        {
            byte number = Convert.ToByte(2);
            ISIStrxvuFrameLengths fls = new ISIStrxvuFrameLengths();
            fls.maxAX25frameLengthRX = 2048;
            fls.maxAX25frameLengthTX = 2048;
            ISIStrxvuFrameLengths fls2 = new ISIStrxvuFrameLengths();
            fls2.maxAX25frameLengthRX = 2048;
            fls2.maxAX25frameLengthTX = 2048;
            ISIStrxvuFrameLengths[] fl = new ISIStrxvuFrameLengths[] { fls, fls2 };
            isisTRXVU.IsisTrxvu_initialize(new ISIStrxvuI2CAddress[] { new ISIStrxvuI2CAddress(), new ISIStrxvuI2CAddress() }, fl, ISIStrxvuBitrate.trxvu_bitrate_2400, number);
            logs.ItemsSource = isisTRXVU.logs;
        }

        public TRXTab(IsisTRXVU isisTRXVU)
        {
            InitializeComponent();
            this.isisTRXVU = isisTRXVU;
            rxBitRateSelect.ItemsSource = Enum.GetValues(typeof(ISIStrxvuBitrateStatus)).Cast<ISIStrxvuBitrateStatus>();
            txBitRateSelect.ItemsSource = Enum.GetValues(typeof(ISIStrxvuBitrateStatus)).Cast<ISIStrxvuBitrateStatus>();

            var logThread = new Thread(() =>
            {
            while (true){
                    this.Dispatcher.Invoke(() =>
                    {
                        logs.Items.Refresh();
                    });
                }
            });

            logThread.IsBackground = true;
            logThread.Start();

            initiallizeTRX();
            trxes.ItemsSource = isisTRXVU.tRXesCollection;
        }

        private void trx_Loaded(object sender, RoutedEventArgs e)
        {
            trxes.ItemsSource = trxesList;
        }

        private void trx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rtxCurrText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            rxDopplerText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            rxCurrText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            busVoltText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            boardTempText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            paTempText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            rssiText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            rxBitrateText.DataContext = ((TRX)trxes.SelectedItem).receiver;

            txReflpwrText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            paTempText1.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            txFwrdpwrText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            txCurrText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            txBitrateText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            idleStateText.DataContext = ((TRX)trxes.SelectedItem).transmitter;

            transmitter_buffer.ItemsSource = ((TRX)trxes.SelectedItem).transmitter.txFrameBuffer.frames.queueCollection;
            receiver_buffer.ItemsSource = ((TRX)trxes.SelectedItem).receiver.rxFrameBuffer.frames.queueCollection;

            maxAX25frameLengthTXText.DataContext = ((TRX)trxes.SelectedItem).maxFrameLengths;
            maxAX25frameLengthRXText.DataContext = ((TRX)trxes.SelectedItem).maxFrameLengths;

            beaconStatusText.DataContext = ((TRX)trxes.SelectedItem);
            intervalText.DataContext = ((TRX)trxes.SelectedItem);
        }

        private void txFrame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (transmitter_buffer.SelectedItem != null)
            {
                currFrame = (AX25Frame)transmitter_buffer.SelectedItem;
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
            else
            {
                txReflpwrText1.Text = "";
                paTempText2.Text = "";
                txFwrdpwrText1.Text = "";
                txCurrText1.Text = "";
            }
            
        }

        private void rxFrame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (receiver_buffer.SelectedItem != null)
            {
                currFrame = (AX25Frame)receiver_buffer.SelectedItem;
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
            else
            {
                txReflpwrText1.Text = "";
                paTempText2.Text = "";
                txFwrdpwrText1.Text = "";
                txCurrText1.Text = "";
            }
            
        }

        private void trx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (trxes.SelectedItem != null)
            {
                ((TRX)trxes.SelectedItem).OverflowTransmitterBuffer();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (trxes.SelectedItem != null)
            {
                ((TRX)trxes.SelectedItem).OverflowReceiverBuffer();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (trxes.SelectedItem != null)
            {
                ((TRX)trxes.SelectedItem).clearTransmitterBuffer();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (trxes.SelectedItem != null)
            {
                ((TRX)trxes.SelectedItem).clearReceiverBuffer();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (trxes.SelectedItem != null)
            {
                currFrame = new AX25Frame("".ToCharArray(), "".ToCharArray(), Encoding.UTF8.GetBytes(""));
                ((TRX)trxes.SelectedItem).receiver.addFrame(currFrame);
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currFrame != null)
            {
            }
        }

        private void maxAX25frameLengthTXText_TextChanged(object sender, TextChangedEventArgs e)
        {
            uint maxLen;
            if (uint.TryParse(maxAX25frameLengthTXText.Text, out maxLen))
            {
                ISIStrxvuFrameLengths fls = new ISIStrxvuFrameLengths();
                fls.maxAX25frameLengthTX = maxLen;
                fls.maxAX25frameLengthRX = ((TRX)trxes.SelectedItem).maxFrameLengths.maxAX25frameLengthRX;
                ((TRX)trxes.SelectedItem).maxFrameLengths = fls;
            }
            else
            {
                maxAX25frameLengthTXText.DataContext = ((TRX)trxes.SelectedItem).maxFrameLengths;
            }
            
        }

        private void maxAX25frameLengthRXText_TextChanged(object sender, TextChangedEventArgs e)
        {
            uint maxLen;
            if (uint.TryParse(maxAX25frameLengthRXText.Text, out maxLen))
            {
                ISIStrxvuFrameLengths fls = new ISIStrxvuFrameLengths();
                fls.maxAX25frameLengthRX = maxLen;
                fls.maxAX25frameLengthTX = ((TRX)trxes.SelectedItem).maxFrameLengths.maxAX25frameLengthTX;
                ((TRX)trxes.SelectedItem).maxFrameLengths = fls;
            }
            else
            {
                maxAX25frameLengthRXText.DataContext = ((TRX)trxes.SelectedItem).maxFrameLengths;
            }
        }

        private void rtxCurrText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort curr;
            if (ushort.TryParse(rtxCurrText.Text, out curr))
            {
                ((TRX)trxes.SelectedItem).receiver.Tx_current = curr;
            }
            else
            {
                rtxCurrText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
            
        }

        private void rxDopplerText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort doppler;
            if (ushort.TryParse(rxDopplerText.Text, out doppler))
            {
                ((TRX)trxes.SelectedItem).receiver.Rx_doppler = doppler;
            }
            else
            {
                rxDopplerText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
        }

        private void rxCurrText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort curr;
            if (ushort.TryParse(rxCurrText.Text, out curr))
            {
                ((TRX)trxes.SelectedItem).receiver.Rx_current = curr;
            }
            else
            {
                rxCurrText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
        }

        private void busVoltText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort busvolt;
            if (ushort.TryParse(busVoltText.Text, out busvolt))
            {
                ((TRX)trxes.SelectedItem).receiver.Bus_volt = busvolt;
            }
            else
            {
                busVoltText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
        }

        private void boardTempText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort boardTemp;
            if (ushort.TryParse(boardTempText.Text, out boardTemp))
            {
                ((TRX)trxes.SelectedItem).receiver.Board_temp = boardTemp;
            }
            else
            {
                boardTempText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
        }

        private void paTempText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort paTemp;
            if (ushort.TryParse(paTempText.Text, out paTemp))
            {
                ((TRX)trxes.SelectedItem).receiver.Pa_temp = paTemp;
            }
            else
            {
                paTempText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
        }

        private void rssiText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort rssi;
            if (ushort.TryParse(rssiText.Text, out rssi))
            {
                ((TRX)trxes.SelectedItem).receiver.Rx_rssi = rssi;
            }
            else
            {
                rssiText.DataContext = ((TRX)trxes.SelectedItem).receiver;
            }
        }
        
        private void rxBitRateSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ISIStrxvuBitrateStatus selected = (ISIStrxvuBitrateStatus)rxBitRateSelect.SelectedValue;
            if (trxes.SelectedItem != null)
            {
                ((TRX)trxes.SelectedItem).receiver.RxBitrate = selected;
            }
        }

        private void txBitRateSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ISIStrxvuBitrateStatus selected = (ISIStrxvuBitrateStatus)txBitRateSelect.SelectedValue;
            if (trxes.SelectedItem != null)
            {
                ((TRX)trxes.SelectedItem).transmitter.TxBitrate = selected;
            }
        }

        private void txReflpwrText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort reflpwr;
            if (ushort.TryParse(txReflpwrText.Text, out reflpwr))
            {
                ((TRX)trxes.SelectedItem).transmitter.Tx_reflpwr = reflpwr;
            }
            else
            {
                txReflpwrText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            }
        }

        private void paTempText1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort paTemp;
            if (ushort.TryParse(paTempText1.Text, out paTemp))
            {
                ((TRX)trxes.SelectedItem).transmitter.Pa_temp = paTemp;
            }
            else
            {
                paTempText1.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            }
        }

        private void txFwrdpwrText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort fwrdpwr;
            if (ushort.TryParse(txFwrdpwrText.Text, out fwrdpwr))
            {
                ((TRX)trxes.SelectedItem).transmitter.Tx_fwrdpwr = fwrdpwr;
            }
            else
            {
                txFwrdpwrText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            }
        }

        private void txCurrText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ushort curr;
            if (ushort.TryParse(txCurrText.Text, out curr))
            {
                ((TRX)trxes.SelectedItem).transmitter.Tx_current = curr;
            }
            else
            {
                txCurrText.DataContext = ((TRX)trxes.SelectedItem).transmitter;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (currFrame != null)
            {
                currFrame.Header.Src = Encoding.ASCII.GetBytes(txReflpwrText1.Text);
                currFrame.computeLength();
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (currFrame != null)
            {
                currFrame.Header.Dest = Encoding.ASCII.GetBytes(paTempText2.Text);
                currFrame.computeLength();
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (currFrame != null)
            {
                currFrame.FrameCheckSeq = Encoding.ASCII.GetBytes(txFwrdpwrText1.Text);
                currFrame.computeLength();
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (currFrame != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(txCurrText1.Text);
                currFrame.infoFeild = data;
                currFrame.FrameCheckSeq = currFrame.calculateCheckSum(data);
                currFrame.computeLength();
                txReflpwrText1.Text = Encoding.UTF8.GetString(currFrame.Header.Src);
                paTempText2.Text = Encoding.UTF8.GetString(currFrame.Header.Dest);
                txFwrdpwrText1.Text = Encoding.UTF8.GetString(currFrame.FrameCheckSeq);
                txCurrText1.Text = Encoding.UTF8.GetString(currFrame.infoFeild);
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            string path = load_file.Text;
            try
            {
                string text = System.IO.File.ReadAllText(path);
                txCurrText1.Text = text;

            }
            catch (Exception)
            {
                MessageBox.Show("File does not exist");
            }
            
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            string path = extract_file.Text;
            try
            {
                System.IO.File.WriteAllLines(path, isisTRXVU.logs.ToArray<String>());
            }
            catch(Exception)
            {
                MessageBox.Show("File does not exist");
            }
        }
    }

    
}
