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
        private IsisTRXVU isisTRXVU;

        public TRXTab()
        {
            InitializeComponent();
            isisTRXVU = new IsisTRXVU();
            byte number = Convert.ToByte(2);
            ISIStrxvuFrameLengths fls = new ISIStrxvuFrameLengths();
            fls.maxAX25frameLengthRX = 2048;
            fls.maxAX25frameLengthTX = 2048;
            ISIStrxvuFrameLengths fls2 = new ISIStrxvuFrameLengths();
            fls2.maxAX25frameLengthRX = 2048;
            fls2.maxAX25frameLengthTX = 2048;
            ISIStrxvuFrameLengths[] fl = new ISIStrxvuFrameLengths[] { fls, fls2};
            ISIStrxvuBitrate iSIStrxvuBitrate = new ISIStrxvuBitrate();

            isisTRXVU.IsisTrxvu_initialize(new ISIStrxvuI2CAddress[] { new ISIStrxvuI2CAddress(), new ISIStrxvuI2CAddress() }, fl, ISIStrxvuBitrate.trxvu_bitrate_2400, number);
            trxesList = isisTRXVU.tRXes;
        }

        private void trx_Loaded(object sender, RoutedEventArgs e)
        {
            trxes.ItemsSource = trxesList;
        }

        private void trx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void trx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
