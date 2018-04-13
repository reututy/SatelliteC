using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    class EPSConstants
    {
        public const int I2C_WDT_RESET_0 = 0;
        public const int I2C_WDT_RESET_1 = 1;
        public const int WDT_I2C_INIT_TIME = 10; //sec
        public const int WDT_GND_INIT_TIME = 480; //sec - should be configurable to  48 hours in the real system
        public const int WDT_GND_HOUR = 10; //sec - should be 1 hour
        public const int WDT_CSP_INIT_PING = 5; //num of pings

    }
}
