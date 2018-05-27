using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class LogicMain
    {
        private GomEPS epsManager;
        private IsisTRXVU trxManager;

        public LogicMain()
        {
            epsManager = new GomEPS();
            trxManager = new IsisTRXVU();
        }
    }
}
