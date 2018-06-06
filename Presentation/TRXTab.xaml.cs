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
            ISIStrxvuBitrate iSIStrxvuBitrate = new ISIStrxvuBitrate();
            isisTRXVU.IsisTrxvu_initialize(new ISIStrxvuI2CAddress[] { new ISIStrxvuI2CAddress(), new ISIStrxvuI2CAddress() }, fl, ISIStrxvuBitrate.trxvu_bitrate_2400, number);
            logs.ItemsSource = isisTRXVU.logs;
        }

        public TRXTab(IsisTRXVU isisTRXVU)
        {
            InitializeComponent();
            this.isisTRXVU = isisTRXVU;
            testt = new TestThreadGui();

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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currFrame != null)
            {
            }
        }

    }

    
}
