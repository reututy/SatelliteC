using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class TestThreadGui : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private String _testt;
        private int counter;

        public String Testt
        {
            get
            {
                return _testt;
            }
            set
            {
                _testt = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Testt"));
                }
            }
        }

        public TestThreadGui()
        {
            _testt = "0";
        }
    }
}
