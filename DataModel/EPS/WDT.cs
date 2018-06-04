namespace DataModel.EPS
{
    /*wdt types*/
    public enum wdt_type { I2C, GND, CSP0, CSP1 }

    public class WDT
    {
        public wdt_type WdtType { get; set; }
        public uint RebootCounter { get; set; }
        public uint TimePingLeft { get; set; }
        public uint data { get; set; } //I2C- type of reset, GND- last hour, CSP- channel connected

        public WDT(wdt_type type, uint reboot, uint left, uint dat)
        {
            WdtType = type;
            RebootCounter = reboot;
            TimePingLeft = left;
            data = dat;
        }
    }
}
