using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /*wdt types*/
    public enum wdt_type { I2C, GND, CSP0, CSP1 }

    public class WDT
    {
        private wdt_type wdt_type { get; set; }
        private uint reboot_counter { get; set; }
        private uint time_ping_left { get; set; }
        private uint data { get; set; } //I2C- type of reset, GND- last hour, CSP- channel connected

        public WDT(wdt_type type, uint reboot, uint left, uint dat)
        {
            wdt_type = type;
            reboot_counter = reboot;
            time_ping_left = left;
            data = dat;
        }
    }
}
