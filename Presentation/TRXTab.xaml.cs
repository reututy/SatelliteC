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
        }

        private void trx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    }

    
}
